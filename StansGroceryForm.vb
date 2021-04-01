'Kendall Callister
'RCET0265
'Spring 2021
'Stans Grocery
'https://github.com/callkend/StansGrocery.git

Option Explicit On
Option Strict On

Public Class StansGroceryForm
    Dim temp() As String
    Dim numberOfAisles As Integer = 0
    Dim catergories(0) As String
    Dim splitter As String

    'Handles the reading the file, and sets tooltips on startup.
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadEntireFile("", False)
        temp = Split(My.Resources.Grocery, vbLf)
        For i = 0 To temp.Length - 1
            Try
                splitter += Mid(temp(i), InStrRev(temp(i), "CAT") + 3, temp(i).Length - (InStrRev(temp(i), "CAT") + 3)) + ","
                If numberOfAisles < CInt(Mid(temp(i), InStrRev(temp(i), "LOC") + 3, 1)) Then
                    numberOfAisles = CInt(Mid(temp(i), InStrRev(temp(i), "LOC") + 3, 1))
                ElseIf numberOfAisles < CInt(Mid(temp(i), InStrRev(temp(i), "LOC") + 3, 2)) Then
                    numberOfAisles = CInt(Mid(temp(i), InStrRev(temp(i), "LOC") + 3, 2))
                End If

            Catch ex As Exception

            End Try
            MainToolTip.SetToolTip(SearchTextBox, "Type in the desired item")
            MainToolTip.SetToolTip(SearchButton, "Search for Item")
            MainToolTip.SetToolTip(FilterByAisleRadioButton, "Filters Items by Aisle")
            MainToolTip.SetToolTip(FilterByCategoryRadioButton, "Filter Items by Category")
            MainToolTip.SetToolTip(FilterComboBox, "Select Aisle or Category to Search")
            MainToolTip.SetToolTip(DisplayListBox, "Displays Filtered Items to pick from")
            MainToolTip.SetToolTip(DisplayLabel, "Displays Items information")

        Next

        temp = Split(splitter, ",")
        Dim arrayScanner As String = ""
        Dim pointer As Integer = 0

        For i = 0 To temp.Length - 1

            If temp(i) <> arrayScanner And temp(i) <> "" Then
                arrayScanner = temp(i)
                ReDim Preserve catergories(pointer)
                catergories(pointer) = temp(i)
                pointer += 1
            End If
        Next
    End Sub

    'Reads the info from the file
    Sub ReadEntireFile(grabItem As String, filter As Boolean)

        Dim scan As String = "nothing"
        Dim itemInfo As String

        temp = Split(My.Resources.Grocery, vbLf)

        'Fills the display box with all items
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
        ElseIf filter Then
            'Displays filtered items
            DisplayListBox.Items.Clear()
            For i = 0 To temp.Length - 1
                If InStrRev(temp(i), grabItem) > 0 Then
                    scan = Mid(temp(i), InStrRev(temp(i), "ITM") + 3, InStr(temp(i), ",") - 8)
                    DisplayListBox.Items.Add(scan)
                End If
            Next
        Else
            Try
                FileOpen(1, "Grocery.txt", OpenMode.Input)

                'Cloes program if zzz is searched
                If grabItem = "Zzz" Then
                    Me.Close()
                End If
                'Grabs Selected Item and tells user its information
                Do Until EOF(1)
                    Input(1, scan)
                    If (InStrRev(scan, grabItem) >= 1) And filter = False Then
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
                    ElseIf filter = False Then
                        DisplayLabel.Text = $"Sorry no matches for {grabItem}"
                    End If

                Loop
                FileClose()
            Catch ex As System.IO.FileNotFoundException

            End Try
        End If

    End Sub

    'Handles item being selected in the display list box
    Private Sub DisplayListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DisplayListBox.SelectedIndexChanged
        Dim itemSelect As String = DisplayListBox.GetItemText(DisplayListBox.SelectedItem)
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        ReadEntireFile(itemSelect, False)

    End Sub

    'Handles item being selected in the display list box
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilterComboBox.SelectedIndexChanged
        Dim itemSelect As String = FilterComboBox.GetItemText(FilterComboBox.SelectedItem)
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        'Turns off filters on Combobox
        If itemSelect = "Show All" Then
            FilterByAisleRadioButton.Checked = False
            FilterByCategoryRadioButton.Checked = False
            FilterComboBox.Items.Clear()
            ReadEntireFile("", False)

            'Checks to see if a filter has been selected
        ElseIf FilterByAisleRadioButton.Checked = False And FilterByCategoryRadioButton.Checked = False Then
            ReadEntireFile(itemSelect, False)
        ElseIf FilterByAisleRadioButton.Checked = True Or FilterByCategoryRadioButton.Checked = True Then
            ReadEntireFile(itemSelect, True)
        End If

    End Sub

    'Searches for the item in the SearchTextBox
    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click, SearchToolStripMenuItem.Click, SearchToolStripMenuItem1.Click
        Dim itemSelect As String
        Try
            itemSelect = SearchTextBox.Text.Substring(0, 1).ToUpper() +
            SearchTextBox.Text.Substring(1)
        Catch
            MsgBox("No Search Entry")
            Return
        End Try
        itemSelect = Mid(itemSelect, 1, Len(itemSelect))
        ReadEntireFile(itemSelect, False)
    End Sub

    'Displays Aisles to search in ComboBox
    Private Sub FilterByAisleRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles FilterByAisleRadioButton.CheckedChanged
        If FilterByAisleRadioButton.Checked = False Then
            Return
        Else
            FilterComboBox.Items.Clear()
            FilterComboBox.Items.Add("Show All")
            For i = 0 To numberOfAisles
                FilterComboBox.Items.Add(i)
            Next
        End If
    End Sub

    'Displays Catergory to search in ComboBox
    Private Sub FilterByCategoryRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles FilterByCategoryRadioButton.CheckedChanged
        If FilterByCategoryRadioButton.Checked = False Then
            Return
        Else
            FilterComboBox.Items.Clear()
            FilterComboBox.Items.Add("Show All")
            For i = 0 To catergories.Length - 1
                FilterComboBox.Items.Add(catergories(i))
            Next
        End If
    End Sub

    'Handles Right click for the Context Menu
    Private Sub Mouse_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Dim mousePosition As Point = New Point(e.X + Me.Location.X, e.Y + Me.Location.Y)

        If e.Button.ToString = "Right" Then
            ContextMenuStrip1.Show(mousePosition)
        End If
    End Sub

    'Handles if exit is clicked in top menu
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

End Class
