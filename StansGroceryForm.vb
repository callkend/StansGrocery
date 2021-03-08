Public Class StansGroceryForm
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadEntireFile("")
    End Sub

    Sub ReadEntireFile(grabItem As String)
        Dim scan As String = "nothing"
        Dim itemInfo As String
        If grabItem = "" Then
            Try
                FileOpen(1, "Grocery.txt", OpenMode.Input)
                Do Until EOF(1)
                    Input(1, scan)
                    If InStrRev(scan, "ITM") <= 5 And InStrRev(scan, "ITM") <> 0 Then
                        scan = Replace(scan, "$$ITM", "")
                        scan = Replace(scan, vbLf, "")
                        scan = Replace(scan, """", "")
                        DisplayListBox.Items.Add(scan)
                        ComboBox1.Items.Add(scan)
                    End If
                Loop
                FileClose(1)
            Catch ex As System.IO.FileNotFoundException

            End Try
        Else
            Try
                FileOpen(1, "Grocery.txt", OpenMode.Input)
                Do Until EOF(1)
                    Input(1, scan)
                    If (InStrRev(scan, grabItem) >= 1) Then
                        scan = Replace(scan, "$$ITM", "")
                        scan = Replace(scan, vbLf, "")
                        scan = Replace(scan, """", "")
                        itemInfo = $"Item:{Mid(scan, 1)}" + vbCrLf
                        Input(1, scan)
                        scan = Replace(scan, "##LOC", "")
                        itemInfo += $"Location:{scan}" + vbCrLf
                        Input(1, scan)
                        scan = Replace(scan, "%%CAT", "")
                        itemInfo += $"Category:{scan}"
                        MsgBox(itemInfo)
                        FileClose(1)
                        Return
                    End If
                Loop
                FileClose()
            Catch ex As System.IO.FileNotFoundException

            End Try
        End If

    End Sub

    Private Sub DisplayListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DisplayListBox.SelectedIndexChanged
        Dim itemSelect As String = DisplayListBox.GetItemText(DisplayListBox.SelectedItem)
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        ReadEntireFile(itemSelect)

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim itemSelect As String = ComboBox1.GetItemText(ComboBox1.SelectedItem)
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        ReadEntireFile(itemSelect)

    End Sub

    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Dim itemSelect As String = TextBox1.Text
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        ReadEntireFile(itemSelect)
    End Sub
End Class
