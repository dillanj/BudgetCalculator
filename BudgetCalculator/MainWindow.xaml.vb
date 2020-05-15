Imports System.IO

Class MainWindow

    Public weekly_expenses As Integer = 0
    Public monthly_expenses As Integer = 0
    Public annual_expenses As Integer = 0






    Private Sub Salary_checkbox_Checked(sender As Object, e As RoutedEventArgs) Handles salary_checkbox.Checked
        salary_input_box.IsEnabled = True
        hourly_input_box.IsEnabled = False
        hours_per_week_input.IsEnabled = False

    End Sub

    Private Sub Salary_checkbox_Unchecked(sender As Object, e As RoutedEventArgs) Handles salary_checkbox.Unchecked
        salary_input_box.IsEnabled = False
        hourly_input_box.IsEnabled = True
        hours_per_week_input.IsEnabled = True
    End Sub



    Private Sub calculateEarnings()
        Dim taxes As Double = taxes_input_box.Text * 0.01

        If hourly_input_box.IsEnabled Then ' HOURLY STATEMENTS
            'check to see if numbers
            Dim hourly_wage As Double
            hourly_wage = hourly_input_box.Text
            Dim hours_worked As Double
            If hours_per_week_input.Text Is "0" Then
                hours_worked = 0
            Else
                hours_worked = hours_per_week_input.Text
            End If

            Dim total_made_per_week = (hourly_wage * hours_worked)
            'TODO: check to see if numbers
            ' TODO: change colors depending on if > 0
            _1_1_label.Content = total_made_per_week
            _2_1_label.Content = total_made_per_week * 4
            _3_1_label.Content = total_made_per_week * 52

            ' tax labels
            Dim total_weekly_taxes = (hourly_wage * hours_worked) * taxes
            _1_2_label.Content = Math.Truncate(total_weekly_taxes)
            _2_2_label.Content = Math.Truncate(total_weekly_taxes * 4)
            _3_2_label.Content = Math.Truncate((total_weekly_taxes * 4) * 12)

        Else ' SALARY STATEMENTS
            'TODO: check to see if numbers
            ' TODO: change colors depending on if > 0
            Dim salary As Double = salary_input_box.Text
            _1_1_label.Content = Math.Truncate(salary / 52)
            _2_1_label.Content = Math.Truncate((salary / 52) * 4)
            _3_1_label.Content = Math.Truncate(salary)

            ' tax labels
            _1_2_label.Content = Math.Truncate((salary * taxes) / 52)
            _2_2_label.Content = Math.Truncate(((salary * taxes) / 52) * 4)
            _3_2_label.Content = Math.Truncate((((salary * taxes) / 52) * 4) * 12)


        End If
        updateTotals()

    End Sub

    Private Sub Calculate_button_Click(sender As Object, e As RoutedEventArgs) Handles calculate_button.Click
        calculateEarnings()
    End Sub

    Private Sub add_button_Click(sender As Object, e As RoutedEventArgs) Handles add_button.Click
        expense_list.Items.Add(expense_textbox.Text)
        Dim amount As String = "$" + expense_amount_textbox.Text
        amount_list.Items.Add(amount)
        ' clear textbox
        expense_amount_textbox.Text = ""
        expense_textbox.Text = ""
        Dim count As Integer = expense_list.Items.Count
        If count = 0 Then
            save_button.IsEnabled = False
        Else
            save_button.IsEnabled = True

        End If
        updateExpenses()
        updateTotals()
    End Sub

    Private Sub updateExpenses()
        weekly_expenses = 0
        monthly_expenses = 0
        annual_expenses = 0
        Dim count As Integer = amount_list.Items.Count
        For i = 0 To count - 1
            monthly_expenses += amount_list.Items(i)
        Next
        weekly_expenses = monthly_expenses / 4
        annual_expenses = monthly_expenses * 12

        expenses_weekly_label.Content = weekly_expenses
        expenses_monthly_label.Content = monthly_expenses
        expenses_annual_label.Content = annual_expenses

    End Sub

    Private Sub delete_button_Click(sender As Object, e As RoutedEventArgs) Handles delete_button.Click
        ' delete both list item at selected item index
        Dim pos As Integer = expense_list.SelectedIndex
        expense_list.Items.RemoveAt(pos)
        amount_list.Items.RemoveAt(pos)
        delete_button.IsEnabled = False
        Dim count As Integer = expense_list.Items.Count
        If count = 0 Then
            save_button.IsEnabled = False
        Else
            save_button.IsEnabled = True

        End If
        updateExpenses()
        updateTotals()
    End Sub

    Private Sub expense_list_GotFocus(sender As Object, e As RoutedEventArgs) Handles expense_list.GotFocus
        delete_button.IsEnabled = True
    End Sub

    Private Sub expense_list_LostFocus(sender As Object, e As RoutedEventArgs) Handles expense_list.LostFocus
        If delete_button.IsEnabled Then
            delete_button.IsEnabled = False
        End If
    End Sub

    Private Sub delete_button_GotFocus(sender As Object, e As RoutedEventArgs) Handles delete_button.GotFocus
        delete_button.IsEnabled = True
    End Sub

    Private Sub delete_button_LostFocus(sender As Object, e As RoutedEventArgs) Handles delete_button.LostFocus
        If delete_button.IsEnabled Then
            delete_button.IsEnabled = False
        End If
    End Sub
    Private Sub save_button_Click(sender As Object, e As RoutedEventArgs) Handles save_button.Click
        Dim fout As StreamWriter = File.CreateText("expenses.txt")
        Dim fout2 As StreamWriter = File.CreateText("amounts.txt")
        Dim count As Integer = expense_list.Items.Count
        fout.WriteLine(count)
        fout2.WriteLine(count)


        'for loops go up to inclusive
        For i = 0 To count - 1
            fout.WriteLine(expense_list.Items(i))
            fout2.WriteLine(amount_list.Items(i))
        Next

        fout.Close()
        fout2.Close()

    End Sub

    Private Sub expense_textbox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles expense_textbox.TextChanged
        If expense_textbox.Text = Nothing Or expense_amount_textbox.Text = Nothing Then
            add_button.IsEnabled = False
            Return
        Else
            add_button.IsEnabled = True
        End If
    End Sub

    Private Sub expense_amount_textbox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles expense_amount_textbox.TextChanged
        If expense_textbox.Text = Nothing Or expense_amount_textbox.Text = Nothing Then
            add_button.IsEnabled = False
            Return
        Else
            add_button.IsEnabled = True
        End If
    End Sub


    Private Sub updateTotals()
        Dim total_a As Double = _3_1_label.Content - expenses_annual_label.Content - _3_2_label.Content
        Dim total_m As Double = _2_1_label.Content - expenses_monthly_label.Content - _2_2_label.Content
        Dim total_w As Double = _1_1_label.Content - expenses_weekly_label.Content - _1_2_label.Content
        total_a_label.Content = total_a
        total_m_label.Content = total_m
        total_w_label.Content = total_w
    End Sub

    Private Sub Load_expenses_button_Click(sender As Object, e As RoutedEventArgs) Handles load_expenses_button.Click
        expense_list.Items.Clear()
        amount_list.Items.Clear()


        Dim fin As StreamReader = File.OpenText("expenses.txt")
        Dim fin2 As StreamReader = File.OpenText("amounts.txt")
        Dim count As Integer
        Dim count2 As Integer
        count = fin.ReadLine
        count2 = fin2.ReadLine
        'TODO: clear list items  before loading
        For i = 1 To count
            Dim expense As String
            Dim amount As String
            expense = fin.ReadLine
            amount = fin2.ReadLine
            expense_list.Items.Add(expense)
            amount_list.Items.Add(amount)

        Next
        fin.Close()
        fin2.Close()
        updateExpenses()
        updateTotals()

    End Sub
End Class
