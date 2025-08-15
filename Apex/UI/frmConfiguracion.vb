' Apex/UI/frmConfiguracion.vb
Public Class frmConfiguracion

    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarTextosDeReglas()
    End Sub

    Private Sub CargarTextosDeReglas()
        ' --- REGLAS DE VIÁTICOS ---
        Dim sbViaticos As New System.Text.StringBuilder()

        sbViaticos.AppendLine("El cálculo de viáticos se basa en las siguientes condiciones para el período mensual seleccionado:")
        sbViaticos.AppendLine()
        sbViaticos.AppendLine(" • Liquidación Normal:")
        sbViaticos.AppendLine("   Se abona el total de días de viático correspondientes al puesto del funcionario.")
        sbViaticos.AppendLine()
        sbViaticos.AppendLine(" • Alta en el período:")
        sbViaticos.AppendLine("   Si un funcionario ingresa durante el mes, se calculan los días de viático de forma proporcional a los días efectivamente trabajados.")
        sbViaticos.AppendLine()
        sbViaticos.AppendLine(" • Baja de la unidad:")
        sbViaticos.AppendLine("   Si un funcionario es dado de baja, se calculan los viáticos de forma proporcional hasta su último día de trabajo.")
        sbViaticos.AppendLine()
        sbViaticos.AppendLine(" • Baja por licencia:")
        sbViaticos.AppendLine("   El pago del viático se anula (0 días) si el funcionario presenta CUALQUIER licencia cuyo tipo suspenda este beneficio (sin importar la duración de la misma).")

        txtReglasViaticos.Text = sbViaticos.ToString()

        ' --- Aquí puedes añadir la carga para otras reglas en el futuro ---
        ' Ejemplo: txtReglasNotificaciones.Text = "..."
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub
End Class