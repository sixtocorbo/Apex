'------------------------------------------------------------------------------
' <auto-generated>
'     Este código se generó a partir de una plantilla.
'
'     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
'     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class Funcionario
    Public Property Id As Integer
    Public Property CI As String
    Public Property Nombre As String
    Public Property Foto As Byte()
    Public Property FechaIngreso As Date
    Public Property TipoFuncionarioId As Integer
    Public Property CargoId As Nullable(Of Integer)
    Public Property Activo As Boolean
    Public Property CreatedAt As Date
    Public Property UpdatedAt As Nullable(Of Date)
    Public Property EstadoId As Nullable(Of Integer)
    Public Property SeccionId As Nullable(Of Integer)
    Public Property EstadoCivilId As Nullable(Of Integer)
    Public Property GeneroId As Nullable(Of Integer)
    Public Property NivelEstudioId As Nullable(Of Integer)
    Public Property PuestoTrabajoId As Nullable(Of Integer)
    Public Property TurnoId As Nullable(Of Integer)
    Public Property SemanaId As Nullable(Of Integer)
    Public Property HorarioId As Nullable(Of Integer)
    Public Property LastPictureId As Nullable(Of Integer)
    Public Property FechaNacimiento As Nullable(Of Date)
    Public Property Domicilio As String
    Public Property Email As String
    Public Property EscalafonId As Nullable(Of Integer)
    Public Property FuncionId As Nullable(Of Integer)

    Public Overridable Property Cargo As Cargo
    Public Overridable Property Escalafon As Escalafon
    Public Overridable Property Estado As Estado
    Public Overridable Property EstadoCivil As EstadoCivil
    Public Overridable Property EstadoTransitorio As ICollection(Of EstadoTransitorio) = New HashSet(Of EstadoTransitorio)
    Public Overridable Property Funcion As Funcion
    Public Overridable Property FuncionarioDotacion As ICollection(Of FuncionarioDotacion) = New HashSet(Of FuncionarioDotacion)
    Public Overridable Property FuncionarioSalud As ICollection(Of FuncionarioSalud) = New HashSet(Of FuncionarioSalud)
    Public Overridable Property Genero As Genero
    Public Overridable Property Horario As Horario
    Public Overridable Property NivelEstudio As NivelEstudio
    Public Overridable Property PuestoTrabajo As PuestoTrabajo
    Public Overridable Property Seccion As Seccion
    Public Overridable Property Semana As Semana
    Public Overridable Property Turno As Turno
    Public Overridable Property FuncionarioFotoHistorico As ICollection(Of FuncionarioFotoHistorico) = New HashSet(Of FuncionarioFotoHistorico)
    Public Overridable Property TipoFuncionario As TipoFuncionario
    Public Overridable Property FuncionarioArma As ICollection(Of FuncionarioArma) = New HashSet(Of FuncionarioArma)
    Public Overridable Property FuncionarioChaleco As ICollection(Of FuncionarioChaleco) = New HashSet(Of FuncionarioChaleco)
    Public Overridable Property FuncionarioDispositivo As ICollection(Of FuncionarioDispositivo) = New HashSet(Of FuncionarioDispositivo)
    Public Overridable Property FuncionarioEstadoLegal As ICollection(Of FuncionarioEstadoLegal) = New HashSet(Of FuncionarioEstadoLegal)
    Public Overridable Property FuncionarioObservacion As ICollection(Of FuncionarioObservacion) = New HashSet(Of FuncionarioObservacion)
    Public Overridable Property HistoricoCustodia As ICollection(Of HistoricoCustodia) = New HashSet(Of HistoricoCustodia)
    Public Overridable Property HistoricoLicencia As ICollection(Of HistoricoLicencia) = New HashSet(Of HistoricoLicencia)
    Public Overridable Property HistoricoNocturnidad As ICollection(Of HistoricoNocturnidad) = New HashSet(Of HistoricoNocturnidad)
    Public Overridable Property HistoricoPresentismo As ICollection(Of HistoricoPresentismo) = New HashSet(Of HistoricoPresentismo)
    Public Overridable Property HistoricoViatico As ICollection(Of HistoricoViatico) = New HashSet(Of HistoricoViatico)
    Public Overridable Property Movimiento As ICollection(Of Movimiento) = New HashSet(Of Movimiento)
    Public Overridable Property NotificacionPersonal As ICollection(Of NotificacionPersonal) = New HashSet(Of NotificacionPersonal)
    Public Overridable Property NovedadFuncionario As ICollection(Of NovedadFuncionario) = New HashSet(Of NovedadFuncionario)
    Public Overridable Property Usuario As ICollection(Of Usuario) = New HashSet(Of Usuario)

End Class
