Option Strict On
Option Explicit On

Public Class frmReportePresenciasPorSeccion
    Inherits frmReportePresenciasBase

    Protected Overrides ReadOnly Property Agrupacion As AgrupacionPresencia
        Get
            Return AgrupacionPresencia.Seccion
        End Get
    End Property

    Protected Overrides ReadOnly Property TituloFormulario As String
        Get
            Return "Presentes/Ausentes por Sección"
        End Get
    End Property

    Protected Overrides ReadOnly Property EtiquetaGrupo As String
        Get
            Return "Sección"
        End Get
    End Property

    Protected Overrides ReadOnly Property TooltipAusentes As String
        Get
            Return "Funcionarios activos sin presencia registrada en la fecha consultada (excluye francos)."
        End Get
    End Property
End Class
