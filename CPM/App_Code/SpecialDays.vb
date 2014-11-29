Imports Microsoft.VisualBasic
Imports RJS.Web.WebControl

Namespace PopCalendarSpecialDay

    Public Class SpecialDays

        Public Shared Sub AddHolidays(ByVal _Calendar As RJS.Web.WebControl.PopCalendar)
            _Calendar.AddHoliday(1, 1, 0, "New Year's Day")
            _Calendar.AddHoliday(26, 1, 0, "Chinese New Year")
            _Calendar.AddHoliday(27, 1, 0, "Chinese New Year")
            _Calendar.AddHoliday(9, 3, 0, "Birthday Of Prophet Muhammad S.A.W")
            _Calendar.AddHoliday(1, 5, 0, "Labour Day")
            _Calendar.AddHoliday(9, 5, 0, "Wesak Day")
            _Calendar.AddHoliday(6, 6, 0, "Birthday Of DYMM SPB Yang Dipertuan Agong")
            _Calendar.AddHoliday(31, 8, 0, "National Day")
            _Calendar.AddHoliday(20, 9, 0, "Hari Raya Puasa")
            _Calendar.AddHoliday(21, 9, 0, "Hari Raya Puasa")
            _Calendar.AddHoliday(17, 10, 0, "Deepavali")
            _Calendar.AddHoliday(27, 11, 0, "Hari Raya Haji")
            _Calendar.AddHoliday(18, 12, 0, "Awal Muharram")
            _Calendar.AddHoliday(25, 12, 0, "Christmas")
            _Calendar.AddHoliday(PopCalendar.SequenceEnum.Third, PopCalendar.DayWeekEnum.Sunday, PopCalendar.MonthEnum.June, "Día del Padre")
            _Calendar.ShowCarnival = PopCalendar.ShowCarnivalDefinition.Holiday
            _Calendar.CarnivalText = ""
            _Calendar.ShowGoodFriday = PopCalendar.ShowGoodFridayDefinition.Holiday
            _Calendar.GoodFridayText = "Good Friday"
        End Sub

        Public Shared Sub AddSpecialDays(ByVal _Calendar As RJS.Web.WebControl.PopCalendar)
            _Calendar.AddSpecialDay(1, 1, 0, "Special Day")
            '_Calendar.AddSpecialDay(New Date(1980, 1, 7), 2, PopCalendar.FrequencyEnum.Week, 0, "Día de Pago")
            '_Calendar.AddSpecialDay(PopCalendar.SequenceEnum.Last, PopCalendar.DayWeekEnum.Thursday, PopCalendar.MonthEnum.November, "Día de Acción de Gracia")
        End Sub

    End Class

End Namespace

