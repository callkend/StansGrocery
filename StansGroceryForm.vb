Public Class StansGroceryForm
    Dim temp() As String
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadEntireFile("")
        temp = Split(My.Resources.Grocery, vbNewLine)
    End Sub

    Sub ReadEntireFile(grabItem As String)

        Dim scan As String = "nothing"
        Dim itemInfo As String
        If grabItem = "" Then
            FilterComboBox.Items.Add("Show All")
            Try
                FileOpen(1, "Grocery.txt", OpenMode.Input)
                Dim i = 0
                Do Until EOF(1)
                    Input(1, scan)
                    If InStrRev(scan, "ITM") <= 5 And InStrRev(scan, "ITM") <> 0 Then
                        scan = Replace(scan, "$$ITM", "")
                        scan = Replace(scan, vbLf, "")
                        scan = Replace(scan, """", "")
                        DisplayListBox.Items.Add(scan)
                        FilterComboBox.Items.Add(scan)
                    End If
                Loop
                FileClose(1)
            Catch ex As System.IO.FileNotFoundException

            End Try
        Else
            Try
                FileOpen(1, "Grocery.txt", OpenMode.Input)
                If grabItem = "Zzz" Then
                    Me.Close()
                End If
                Do Until EOF(1)
                    Input(1, scan)
                    If (InStrRev(scan, grabItem) >= 1) Then
                        scan = Replace(scan, "$$ITM", "")
                        scan = Replace(scan, vbLf, "")
                        scan = Replace(scan, """", "")
                        itemInfo = $"You will find {Mid(scan, 1)}"
                        Input(1, scan)
                        scan = Replace(scan, "##LOC", "")
                        itemInfo += $" on aisle {scan}"
                        Input(1, scan)
                        scan = Replace(scan, "%%CAT", "")
                        itemInfo += $" with the {scan}"
                        DisplayLabel.Text = (itemInfo)
                        FileClose(1)
                        Return
                    Else
                        DisplayLabel.Text = $"Sorry no matches for {grabItem}"
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

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilterComboBox.SelectedIndexChanged
        Dim itemSelect As String = FilterComboBox.GetItemText(FilterComboBox.SelectedItem)
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        If itemSelect = "Show All" Then
            FilterByAisleRadioButton.Checked = False
            FilterByCategoryRadioButton.Checked = False
            ReadEntireFile("")
        End If
        ReadEntireFile(itemSelect)

    End Sub

    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Dim itemSelect As String = SearchTextBox.Text.Substring(0, 1).ToUpper() +
            SearchTextBox.Text.Substring(1)
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        ReadEntireFile(itemSelect)
    End Sub

    Private Sub FilterByAisleRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles FilterByAisleRadioButton.CheckedChanged
        If FilterByAisleRadioButton.Checked = False Then
            Return
        Else

        End If
    End Sub
End Class
