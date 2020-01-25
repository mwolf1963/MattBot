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
        private Dictionary<string, Class> classDictionary;
        private DiscordSocketClient client;
        
        private void cClassReminder()
        {
             getClassDictionary();
        }

        private void getClassDictionary()
        {
            classDictionary = new Dictionary<string, Class>();
            classDictionary.Add("sdev250", new Class("sdev250", DayOfWeek.Wednesday, Convert.ToDateTime("17:30:00"), 668545020302721138));
            classDictionary.Add("sdev253c", new Class("sdev253 section C", DayOfWeek.Thursday, Convert.ToDateTime("17:30:00"), 668545156462411796));            
            classDictionary.Add("sdev265", new Class("sdev265",DayOfWeek.Monday, Convert.ToDateTime("18:00:00"), 668545275459272724));            
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

        private void ItsClassTime(Class selectedClass)
        {
            DayOfWeek day = selectedClass.GetClassDay();
            switch (day)
            {
                case DayOfWeek.Monday:
                    SendReminder(selectedClass);
                    break;
                case DayOfWeek.Tuesday:
                    SendReminder(selectedClass);
                    break;
                case DayOfWeek.Wednesday:
                    SendReminder(selectedClass);
                    break;
                case DayOfWeek.Thursday:
                    SendReminder(selectedClass);
                    break;
                case DayOfWeek.Friday:
                    SendReminder(selectedClass);
                     break;
                case DayOfWeek.Saturday:
                    SendReminder(selectedClass);
                    break;
            }
        }

        

        private async void SendReminder(Class selectedClass)
        {
            var channel = client.GetChannel(selectedClass.GetChannelId()) as SocketTextChannel;
            if (channel == null) return;
            selectedClass.ReminderSent();
            reminderNeedsReset = true;
            await channel.SendMessageAsync(selectedClass.GetName() + " starts in 1 hour!");
            

        }

        private class Class
        {
            private String name;
            private ulong channelId;
            private DayOfWeek classDay;
            private DateTime reminderTime;
            private bool hasbeensent;

            public Class(String name, DayOfWeek day, DateTime reminderTime, ulong  channelName)
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
