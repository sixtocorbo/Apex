' Apex/Services/ViaticoService.vb
Imports System.Data.Entity

' Un DTO (Data Transfer Object) para estructurar los resultados del cálculo
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

    ''' <summary>
    ''' Calcula la liquidación de viáticos para un período determinado.
    ''' (VERSIÓN CORREGIDA Y OPTIMIZADA)
    ''' </summary>
    Public Async Function CalcularLiquidacionAsync(periodo As Date) As Task(Of List(Of ViaticoResultadoDTO))
        Dim fechaInicioMesActual = New Date(periodo.Year, periodo.Month, 1)
        Dim fechaFinMesActual = fechaInicioMesActual.AddMonths(1).AddDays(-1)

        ' Se incluyen funcionarios que fueron dados de baja en el período
        Dim funcionarios = Await _unitOfWork.Repository(Of Funcionario)().GetAll().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.Seccion).
            Include(Function(f) f.Escalafon).
            Include(Function(f) f.PuestoTrabajo.TipoViatico).
            Where(Function(f) (f.Activo OrElse (Not f.Activo AndAlso f.UpdatedAt.HasValue AndAlso f.UpdatedAt.Value >= fechaInicioMesActual AndAlso f.UpdatedAt.Value <= fechaFinMesActual)) AndAlso
                              f.Escalafon IsNot Nothing AndAlso
                              f.Escalafon.Nombre = "L" AndAlso
                              f.PuestoTrabajo IsNot Nothing AndAlso
                              f.PuestoTrabajo.TipoViatico IsNot Nothing AndAlso
                              f.PuestoTrabajo.TipoViatico.Dias > 0).
            ToListAsync()

        If Not funcionarios.Any() Then
            Return New List(Of ViaticoResultadoDTO)()
        End If

        Dim funcionariosIds = funcionarios.Select(Function(f) f.Id).ToList()
        Dim licenciasMesActual = Await _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().
            Include(Function(l) l.TipoLicencia).
            Where(Function(l) funcionariosIds.Contains(l.FuncionarioId) AndAlso
                              l.TipoLicencia.SuspendeViatico AndAlso
                              l.inicio <= fechaFinMesActual AndAlso
                              l.finaliza >= fechaInicioMesActual AndAlso
                              l.estado IsNot Nothing AndAlso
                              Not {"Rechazado", "Anulado"}.Contains(l.estado.Trim())).
            ToListAsync()

        Dim resultadoFinal As New List(Of ViaticoResultadoDTO)

        For Each func In funcionarios
            Dim diasBaseViatico = func.PuestoTrabajo.TipoViatico.Dias
            Dim diasAPagar = diasBaseViatico
            Dim motivo = "Normal"
            Dim observaciones = ""

            If Not func.Activo Then
                ' LÓGICA PARA BAJAS
                Dim diasTrabajados = (func.UpdatedAt.Value.Date - fechaInicioMesActual).Days + 1
                Dim totalDiasMes = DateTime.DaysInMonth(periodo.Year, periodo.Month)
                diasAPagar = CInt(Math.Round((diasTrabajados / totalDiasMes) * diasBaseViatico))
                motivo = "Baja de la unidad"

            ElseIf func.FechaIngreso >= fechaInicioMesActual AndAlso func.FechaIngreso <= fechaFinMesActual Then
                ' LÓGICA PARA ALTAS
                Dim diasEnElMes = (fechaFinMesActual - func.FechaIngreso).Days + 1
                Dim totalDiasMes = DateTime.DaysInMonth(periodo.Year, periodo.Month)
                diasAPagar = CInt(Math.Round((diasEnElMes / totalDiasMes) * diasBaseViatico))
                motivo = "Alta en el período"
            Else
                ' LÓGICA PARA LICENCIAS
                Dim licenciasDelFuncionario = licenciasMesActual.Where(Function(l) l.FuncionarioId = func.Id)
                If licenciasDelFuncionario.Any() Then
                    Dim diasDeLicenciaEnMes = licenciasDelFuncionario.Sum(Function(l)
                                                                              Dim inicioLic = If(l.inicio < fechaInicioMesActual, fechaInicioMesActual, l.inicio)
                                                                              Dim finLic = If(l.finaliza > fechaFinMesActual, fechaFinMesActual, l.finaliza)
                                                                              Return (finLic - inicioLic).Days + 1
                                                                          End Function)

                    ' --- INICIO DE LA CORRECCIÓN ---
                    ' Se elimina la restricción de "> 2". Ahora cualquier día de licencia que
                    ' suspenda viático (es decir, diasDeLicenciaEnMes > 0) anula el pago.
                    If diasDeLicenciaEnMes > 0 Then
                        diasAPagar = 0
                        motivo = "Baja por licencia"
                        observaciones = String.Join(", ", licenciasDelFuncionario.Select(Function(l) l.TipoLicencia.Nombre).Distinct())
                    End If
                    ' --- FIN DE LA CORRECCIÓN ---
                End If
            End If

            ' Se añaden todos los movimientos (altas, bajas y liquidaciones normales)
            resultadoFinal.Add(New ViaticoResultadoDTO With {
                .Grado = If(func.Cargo IsNot Nothing AndAlso func.Cargo.Grado.HasValue, func.Cargo.Grado.Value.ToString(), "N/A"),
                .Cedula = func.CI,
                .NombreFuncionario = func.Nombre,
                .DiasAPagar = diasAPagar,
                .Motivo = motivo,
                .Observaciones = observaciones,
                .Seccion = If(func.Seccion IsNot Nothing, func.Seccion.Nombre, "N/A")
            })
        Next

        Return resultadoFinal.OrderBy(Function(r) r.Grado).ThenBy(Function(r) r.NombreFuncionario).ToList()
    End Function

End Class