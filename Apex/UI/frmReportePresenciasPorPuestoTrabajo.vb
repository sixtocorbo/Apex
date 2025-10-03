Option Strict On
Option Explicit On

Public Class frmReportePresenciasPorPuestoTrabajo
    Inherits frmReportePresenciasBase

    Protected Overrides ReadOnly Property Agrupacion As AgrupacionPresencia
        Get
            Return AgrupacionPresencia.PuestoTrabajo
        End Get
    End Property

    Protected Overrides ReadOnly Property TituloFormulario As String
        Get
            Return "Presentes/Ausentes por Puesto de Trabajo"
        End Get
    End Property

    Protected Overrides ReadOnly Property EtiquetaGrupo As String
        Get
            Return "Puesto de trabajo"
        End Get
    End Property

    Protected Overrides ReadOnly Property TooltipAusentes As String
        Get
            Return "Funcionarios activos sin presencia registrada en la fecha consultada (incluye francos, licencias y sin registro)."
        End Get
    End Property
End Class
