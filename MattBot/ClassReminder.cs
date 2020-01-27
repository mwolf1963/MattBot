using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

namespace MattBot
{
    public  class ClassReminder : InfoModule
    {
        private Boolean reminderNeedsReset = false;
        private static readonly object padlock = new object();
        private static ClassReminder instance = null;
        private Dictionary<string, Course> classDictionary;
        private DiscordSocketClient client;
        
        private void cClassReminder()
        {
             getClassDictionary();
        }

        private void getClassDictionary()
        {
            classDictionary = new Dictionary<string, Course>();
            classDictionary.Add("sdev250", new Course("sdev250", DayOfWeek.Wednesday, Convert.ToDateTime("17:30:00"), 668545020302721138));
            classDictionary.Add("sdev253c", new Course("sdev253 section C", DayOfWeek.Thursday, Convert.ToDateTime("17:30:00"), 668545156462411796));            
            classDictionary.Add("sdev265", new Course("sdev265",DayOfWeek.Monday, Convert.ToDateTime("18:00:00"), 668545275459272724));            
        }

        public  ClassReminder(DiscordSocketClient client)
        {            
            {
                lock (padlock)
                {
                    if (instance == null && client.ConnectionState == ConnectionState.Connected)
                    {
                        this.client = client;
                        cClassReminder();
                    }                    
                }
            }
        }
        
        
        public async Task CheckReminder( )
        {
            //Console.WriteLine();
            foreach (var classes in classDictionary)
            {
               // Console.WriteLine("class: " + classes.Key + " time: " + classes.Value.GetClassTime() + " on " + classes.Value.GetClassDay());
               // Console.WriteLine("these are the values of the if statement: "+ (classes.Value.GetClassTime().TimeOfDay > DateTime.Now.TimeOfDay) + " " + classes.Value.GetClassTime().TimeOfDay + " "+ DateTime.Now.TimeOfDay);
                if (((classes.Value.GetClassTime().TimeOfDay < DateTime.Now.TimeOfDay) && (!classes.Value.IsReminderSent())))
                {
                    ItsClassTime(classes.Value);
                } else if (reminderNeedsReset && DateTime.Now.TimeOfDay < Convert.ToDateTime("00:00:01").TimeOfDay)
                {
                    foreach (var resetClass in classDictionary)
                    {
                        resetClass.Value.ResetReminder();
                    }

                    reminderNeedsReset = false;
                }
            }
        }

        private void ItsClassTime(Course selectedCourse)
        {
            DayOfWeek day = selectedCourse.GetClassDay();
            switch (day)
            {
                case DayOfWeek.Monday:
                    SendReminder(selectedCourse);
                    break;
                case DayOfWeek.Tuesday:
                    SendReminder(selectedCourse);
                    break;
                case DayOfWeek.Wednesday:
                    SendReminder(selectedCourse);
                    break;
                case DayOfWeek.Thursday:
                    SendReminder(selectedCourse);
                    break;
                case DayOfWeek.Friday:
                    SendReminder(selectedCourse);
                     break;
                case DayOfWeek.Saturday:
                    SendReminder(selectedCourse);
                    break;
            }
        }

        

        private async void SendReminder(Course selectedCourse)
        {
            var channel = client.GetChannel(selectedCourse.GetChannelId()) as SocketTextChannel;
            if (channel == null) return;
            selectedCourse.ReminderSent();
            reminderNeedsReset = true;
            await channel.SendMessageAsync(selectedCourse.GetName() + " starts in 1 hour!");
            

        }

        private class Course
        {
            private String name;
            private ulong channelId;
            private DayOfWeek classDay;
            private DateTime reminderTime;
            private bool hasbeensent;

            public Course(String name, DayOfWeek day, DateTime reminderTime, ulong  channelName)
            {
                this.name = name;
                this.classDay = day;
                this.reminderTime = reminderTime;
                this.channelId = channelName;
                hasbeensent = false;
            }

            public String GetName()
            {
                return name;
            }
            public DayOfWeek GetClassDay()
            {
                return classDay;
            }
            public DateTime GetClassTime()
            {
                return reminderTime;
            }

            public bool IsReminderSent()
            {
                return hasbeensent;
            }

            public void ResetReminder()
            {
                hasbeensent = false;
            }

            public ulong GetChannelId()
            {
                return channelId;
            }
            public void ReminderSent()
            {
                hasbeensent = true;
            }
        }
    }
    
}
