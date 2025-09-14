' Apex/Services/ViaticoService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks

' DTO para resultados
Public Class ViaticoResultadoDTO
    Public Property Grado As String
    Public Property Cedula As String
    Public Property NombreFuncionario As String
    Public Property DiasAPagar As Integer
    Public Property Motivo As String
    Public Property Observaciones As String
    Public Property Seccion As String
End Class

Public Class ViaticoService
    Inherits GenericService(Of HistoricoViatico)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    ' (opcional) ctor para inyección de UoW si lo necesitás
    Public Sub New(uow As IUnitOfWork)
        MyBase.New(uow)
    End Sub

    ''' <summary>
    ''' Calcula la liquidación de viáticos para un período dado (mes calendario).
    ''' Regla: bajas/altas prorratean; licencias que suspenden viático → 0 días.
    ''' </summary>
    Public Async Function CalcularLiquidacionAsync(periodo As Date) As Task(Of List(Of ViaticoResultadoDTO))
        Dim inicioMes As Date = New Date(periodo.Year, periodo.Month, 1)
        Dim finMes As Date = inicioMes.AddMonths(1).AddDays(-1)
        Dim totalDiasMes As Integer = DateTime.DaysInMonth(periodo.Year, periodo.Month)

        ' 1) Funcionarios elegibles (activos o dados de baja en el período)
        Dim funcionarios = Await _unitOfWork.Repository(Of Funcionario)().GetAll().
            AsNoTracking().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.Seccion).
            Include(Function(f) f.Escalafon).
            Include(Function(f) f.PuestoTrabajo.TipoViatico).
            Where(Function(f) _
                  (f.Activo OrElse (Not f.Activo AndAlso f.UpdatedAt.HasValue AndAlso
                                    f.UpdatedAt.Value >= inicioMes AndAlso f.UpdatedAt.Value <= finMes)) AndAlso
                  f.Escalafon IsNot Nothing AndAlso f.Escalafon.Nombre = "L" AndAlso
                  f.PuestoTrabajo IsNot Nothing AndAlso
                  f.PuestoTrabajo.TipoViatico IsNot Nothing AndAlso
                  f.PuestoTrabajo.TipoViatico.Dias > 0).
            ToListAsync()

        If funcionarios.Count = 0 Then
            Return New List(Of ViaticoResultadoDTO)()
        End If

        ' 2) Licencias que suspenden viático en el período
        Dim funcIds = funcionarios.Select(Function(f) f.Id).ToList()

        Dim licenciasMes = Await _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().
            AsNoTracking().
            Include(Function(l) l.TipoLicencia).
            Where(Function(l) funcIds.Contains(l.FuncionarioId) AndAlso
                              l.TipoLicencia.SuspendeViatico AndAlso
                              l.inicio <= finMes AndAlso
                              l.finaliza >= inicioMes AndAlso
                              l.estado IsNot Nothing AndAlso
                              Not {"Rechazado", "Anulado"}.Contains(l.estado.Trim())).
            ToListAsync()

        Dim resultados As New List(Of ViaticoResultadoDTO)()

        ' 3) Cálculo por funcionario
        For Each f In funcionarios
            Dim diasBase As Integer = f.PuestoTrabajo.TipoViatico.Dias
            Dim diasPagar As Integer = diasBase
            Dim motivo As String = "Normal"
            Dim obs As String = String.Empty

            If Not f.Activo Then
                ' Baja dentro del período
                Dim fechaBaja As Date = If(f.UpdatedAt.HasValue, f.UpdatedAt.Value.Date, inicioMes)
                Dim diasTrabajados As Integer = (fechaBaja - inicioMes).Days + 1
                If diasTrabajados < 0 Then diasTrabajados = 0

                Dim proporcional As Double = (diasTrabajados / CDbl(totalDiasMes)) * diasBase
                diasPagar = CInt(Math.Round(proporcional, MidpointRounding.AwayFromZero))
                motivo = "Baja en el período"

            ElseIf f.FechaIngreso >= inicioMes AndAlso f.FechaIngreso <= finMes Then
                ' Alta dentro del período
                Dim alta As Date = f.FechaIngreso.Date
                Dim diasEnMes As Integer = (finMes - alta).Days + 1
                If diasEnMes < 0 Then diasEnMes = 0

                Dim proporcional As Double = (diasEnMes / CDbl(totalDiasMes)) * diasBase
                diasPagar = CInt(Math.Round(proporcional, MidpointRounding.AwayFromZero))
                motivo = "Alta en el período"

            Else
                ' Licencias que suspenden viático
                Dim licDelFunc = licenciasMes.Where(Function(l) l.FuncionarioId = f.Id).ToList()
                If licDelFunc.Count > 0 Then
                    Dim diasLic As Integer =
                        licDelFunc.Sum(Function(l)
                                           Dim ini As Date = If(l.inicio < inicioMes, inicioMes, l.inicio)
                                           Dim fin As Date = If(l.finaliza > finMes, finMes, l.finaliza)
                                           Return (fin - ini).Days + 1
                                       End Function)
                    If diasLic > 0 Then
                        diasPagar = 0
                        motivo = "Baja por licencia"
                        obs = String.Join(", ", licDelFunc.Select(Function(l) l.TipoLicencia.Nombre).Distinct())
                    End If
                End If
            End If

            resultados.Add(New ViaticoResultadoDTO With {
                .Grado = If(f.Cargo IsNot Nothing AndAlso f.Cargo.Grado.HasValue, f.Cargo.Grado.Value.ToString(), "N/A"),
                .Cedula = f.CI,
                .NombreFuncionario = f.Nombre,
                .DiasAPagar = diasPagar,
                .Motivo = motivo,
                .Observaciones = obs,
                .Seccion = If(f.Seccion IsNot Nothing, f.Seccion.Nombre, "N/A")
            })
        Next

        Return resultados.OrderBy(Function(r) r.Grado).ThenBy(Function(r) r.NombreFuncionario).ToList()
    End Function


End Class
