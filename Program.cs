using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        static readonly NotifyIcon notifyIcon = new NotifyIcon();
        static Timer timer;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // تنظیمات اولیه NotifyIcon
            notifyIcon.Icon = SystemIcons.Application; // Set your icon here
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = CreateContextMenu();

            // تایمر برای به‌روزرسانی روز
            timer = new Timer { Interval = 60000 }; // هر 60 ثانیه بررسی می‌کند
            timer.Tick += (s, e) => UpdateDayTitle();
            timer.Start();

            // به‌روزرسانی اولیه روز
            UpdateDayTitle();

            // اجرا
            Application.Run();
        }

        static void UpdateDayTitle()
        {
            // دریافت تاریخ فعلی به شمسی
            DateTime now = DateTime.Now;
            PersianCalendar persianCalendar = new PersianCalendar();
            string persianDay = persianCalendar.GetDayOfMonth(now).ToString();

            // نمایش روز شمسی در عنوان NotifyIcon
            notifyIcon.Text = $"امروز {persianDay}";
            notifyIcon.BalloonTipText = $"روز {persianDay}";
        }

        static ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            // آیتم تاریخ‌های مختلف
            var persianDateMenuItem = new ToolStripMenuItem(GetPersianDate());
            var arabicDateMenuItem = new ToolStripMenuItem(GetArabicDate());
            var gregorianDateMenuItem = new ToolStripMenuItem(GetGregorianDate());
            contextMenu.Items.Add(persianDateMenuItem);
            contextMenu.Items.Add(arabicDateMenuItem);
            contextMenu.Items.Add(gregorianDateMenuItem);

            // افزودن رویداد کلیک برای کپی به کلیپبورد
            persianDateMenuItem.Click += (s, e) => CopyToClipboard(persianDateMenuItem.Text);
            arabicDateMenuItem.Click += (s, e) => CopyToClipboard(arabicDateMenuItem.Text);
            gregorianDateMenuItem.Click += (s, e) => CopyToClipboard(gregorianDateMenuItem.Text);

            // آیتم ماه‌های تقویم شمسی
            var persianMonthsMenuItem = new ToolStripMenuItem("ماه‌های تقویم شمسی");
            var persianCulture = new CultureInfo("fa-IR");
            foreach (var month in persianCulture.DateTimeFormat.MonthNames)
            {
                if (!string.IsNullOrEmpty(month))
                {
                    var monthItem = new ToolStripMenuItem(month);
                    monthItem.Click += (s, e) => CopyToClipboard(monthItem.Text);
                    persianMonthsMenuItem.DropDownItems.Add(monthItem);
                }
            }
            contextMenu.Items.Add(persianMonthsMenuItem);

            // آیتم ماه‌های تقویم قمری
            var arabicMonthsMenuItem = new ToolStripMenuItem("ماه‌های تقویم قمری");
            var arabicCulture = new CultureInfo("ar-SA");
            foreach (var month in arabicCulture.DateTimeFormat.MonthNames)
            {
                if (!string.IsNullOrEmpty(month))
                {
                    var monthItem = new ToolStripMenuItem(month);
                    monthItem.Click += (s, e) => CopyToClipboard(monthItem.Text);
                    arabicMonthsMenuItem.DropDownItems.Add(monthItem);
                }
            }
            contextMenu.Items.Add(arabicMonthsMenuItem);

            // آیتم ماه‌های تقویم میلادی
            var gregorianMonthsMenuItem = new ToolStripMenuItem("ماه‌های تقویم میلادی");
            var gregorianCulture = new CultureInfo("en-US");
            foreach (var month in gregorianCulture.DateTimeFormat.MonthNames)
            {
                if (!string.IsNullOrEmpty(month))
                {
                    var monthItem = new ToolStripMenuItem(month);
                    monthItem.Click += (s, e) => CopyToClipboard(monthItem.Text);
                    gregorianMonthsMenuItem.DropDownItems.Add(monthItem);
                }
            }
            contextMenu.Items.Add(gregorianMonthsMenuItem);

            contextMenu.Items.Add(new ToolStripSeparator());

            // آیتم خروج
            var exitMenuItem = new ToolStripMenuItem("خروج", null, (s, e) => Application.Exit());
            contextMenu.Items.Add(exitMenuItem);

            return contextMenu;
        }

        static string GetPersianDate()
        {
            var persianCalendar = new PersianCalendar();
            DateTime now = DateTime.Now;
            return $"{persianCalendar.GetDayOfWeek(now)} {persianCalendar.GetDayOfMonth(now)} {persianCalendar.GetMonth(now)} {persianCalendar.GetYear(now)}";
        }

        static string GetArabicDate()
        {
            var arabicCulture = new CultureInfo("ar-SA");
            return DateTime.Now.ToString("dddd d MMMM yyyy", arabicCulture);
        }

        static string GetGregorianDate()
        {
            var gregorianCulture = new CultureInfo("en-US");
            return DateTime.Now.ToString("dddd d MMMM yyyy", gregorianCulture);
        }

        static void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}