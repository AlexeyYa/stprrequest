/*

Copyright 2019 Yamborisov Alexey

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;


using Microsoft.Exchange.WebServices.Data;
using System.Windows.Controls;

namespace Exchange
{
    public static class Emailer
    {
        public static void SendEmail(string from, IEnumerable<string> to, string subject, string body, string attachmentFileName)
        {
            var service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.AutodiscoverUrl(from);
            var message = new EmailMessage(service)
            {
                Subject = subject,
                Body = body,
            };
            message.ToRecipients.AddRange(to);
            message.Attachments.AddFileAttachment(attachmentFileName);
            message.SendAndSaveCopy();
        }

        public static void SendEmail(string from, IEnumerable<string> to, string subject,
            string body, IEnumerable<string> attachmentFileNames)
        {
            var service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.AutodiscoverUrl(from);
            var message = new EmailMessage(service)
            {
                Subject = subject,
                Body = body,
            };
            message.ToRecipients.AddRange(to);
            foreach (var file in attachmentFileNames)
                message.Attachments.AddFileAttachment(file);

            message.SendAndSaveCopy();
        }

        /* SMTP variant
            using System.Net;
            using System.Net.Mail;

        private static void SendSMTP(string smtpServer, string recipient,
            string user, string password, string attachment,
            string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential networkCredential = new NetworkCredential(user, password);
            MailMessage mail = new MailMessage();
            MailAddress fromAddress = new MailAddress(user);

            smtpClient.Host = smtpServer;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
            smtpClient.Timeout = (5 * 1000);
            smtpClient.EnableSsl = true;

            mail.From = fromAddress;
            mail.Subject = subject;
            mail.IsBodyHtml = false;
            mail.Body = body;
            mail.To.Add(recipient);

            if (attachment != null)
                mail.Attachments.Add(new System.Net.Mail.Attachment(attachment));

            smtpClient.Send(mail);
        }*/
    }
}

namespace requestCreator
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            Data = new ObservableCollection<DataClass>();
            Comments = String.Empty;
            Corrections = String.Empty;
            EndDate = DateTime.Now;
            InputDialogVisibility = false;
        }

        private string user;
        public string User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }
        private string group;
        public string Group
        {
            get { return group; }
            set
            {
                if (group != value)
                {
                    group = value;
                    OnPropertyChanged("Group");
                }
            }
        }
        private string obj;
        public string Object
        {
            get { return obj; }
            set
            {
                if (obj != value)
                {
                    if (value != null && value != "")
                        ObjectCh = true;
                    else
                        ObjectCh = false;

                    obj = value;
                    OnPropertyChanged("Object");
                }
            }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }
        private string subs;
        public string Subs
        {
            get { return subs; }
            set
            {
                if (subs != value)
                {
                    if (value == Variables.Instance.Subs[Variables.Instance.Subs.Count - 1]) // If last allow editing
                    {
                        SubsIsEditable = true;
                    }

                    if (value != null && value != "")
                        SubsCh = true;
                    else
                        SubsCh = false;

                    subs = value;
                    OnPropertyChanged("Subs");
                }
            }
        }
        private bool subsIsEditable;
        public bool SubsIsEditable
        {
            get { return subsIsEditable; }
            set
            {
                if (subsIsEditable != value)
                {
                    subsIsEditable = value;
                    OnPropertyChanged("SubsIsEditable");
                }
            }
        }
        private string comments;
        public string Comments
        {
            get { return comments; }
            set
            {
                if (comments != value)
                {
                    comments = value;
                    OnPropertyChanged("Comments");
                }
            }
        }
        private string publishType;
        public string PublishType
        {
            get { return publishType; }
            set
            {
                if (publishType != value)
                {
                    if (value != null && value != "")
                        PublishCh = true;
                    else
                        PublishCh = false;

                    publishType = value;
                    OnPropertyChanged("PublishType");
                }
            }
        }
        private string tasks;
        public string Tasks
        {
            get { return tasks; }
            set
            {
                if (tasks != value)
                {
                    if (value != null && value != "")
                        TasksCh = true;
                    else
                        TasksCh = false;
                 
                    tasks = value;
                    OnPropertyChanged("Tasks");
                }
            }
        }
        private string corrections;
        public string Corrections
        {
            get { return corrections; }
            set
            {
                if (corrections != value)
                {
                    corrections = value;
                    OnPropertyChanged("Corrections");
                }
            }
        }
        private int numberOfOriginals;
        public int NumberOfOriginals
        {
            get { return numberOfOriginals; }
            set
            {
                if (numberOfOriginals != value)
                {
                    numberOfOriginals = value;
                    OnPropertyChanged("NumberOfOriginals");
                }
            }
        }
        private int numberOfCopies;
        public int NumberOfCopies
        {
            get { return numberOfCopies; }
            set
            {
                if (numberOfCopies != value)
                {
                    numberOfCopies = value;
                    OnPropertyChanged("NumberOfCopies");
                }
            }
        }
        private DateTime? endDate;
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        private ObservableCollection<DataClass> data;
        public ObservableCollection<DataClass> Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        private bool recursive;
        public bool Recursive
        {
            get { return recursive; }
            set
            {
                if (recursive != value)
                {
                    recursive = value;
                    OnPropertyChanged("Recursive");
                }
            }
        }

        private bool inputDialogVisibility;
        public bool InputDialogVisibility
        {
            get { return inputDialogVisibility; }
            set { if (inputDialogVisibility != value)
                {
                    inputDialogVisibility = value;
                    OnPropertyChanged("InputDialogVisibility");
                }
            }
        }
        private string recievers;
        public string Recievers
        {
            get { return recievers; }
            set
            {
                if (recievers != value)
                {
                    recievers = value;
                    OnPropertyChanged("Recievers");
                }
            }
        }
        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                    (createCommand = new RelayCommand(obj =>
                    {
                        if (Subs != Variables.Instance.Subs[0])
                        {
                            PdfFormat sum = new PdfFormat();
                            DataClass new_data = new DataClass();
                            new_data.Link = data[0].Link;
                            new_data.DocCode = Subs + "-" + data.Count + "т";
                            foreach (var d in data)
                            {
                                sum += d.Size;
                                Comments += d.DocCode + ";";
                            }
                            new_data.Size = sum;
                            data.Clear();
                            data.Add(new_data);
                        }

                        if (!DocxModule.Create(this, Variables.Instance.Path))
                        {
                            MessageBoxResult messageBox = MessageBox.Show("Не удалось создать файл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            try
                            {
                                var RecieversList = new List<string>(Variables.Instance.RecieversList);
                                if (Recievers != "" && Recievers != null)
                                    RecieversList.AddRange(Recievers.Split(';'));
                                List<string> attachmentFileNames = new List<string>();
                                foreach (var d in data)
                                {
                                    attachmentFileNames.AddRange(DocxModule.GetFilenames());
                                    //email_send(Properties.Settings.Default.Server, Properties.Settings.Default.Reciever,
                                    //    Properties.Settings.Default.Mail, Properties.Settings.Default.Pass,
                                    //    SavePath + d.DocCode + @".docx", "test attachment", "body");
                                }
                                Exchange.Emailer.SendEmail(Properties.Settings.Default.Sender,
                                    RecieversList,
                                    "subj", "body", attachmentFileNames);
                                DocxModule.GetFilenames().Clear();
                                MessageBox.Show("Заявка отправлена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            }
                            catch (Exception e) {
                                MessageBoxResult messageBox = MessageBox.Show("Не удалось отправить файл\n" + e, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    },
                    obj => {
                        return CreateCommandEnabler();
                    }));
            }
        }

        private RelayCommand addNewCommand;
        public RelayCommand AddNewCommand
        {
            get
            {
                return addNewCommand ??
                    (addNewCommand = new RelayCommand(obj =>
                    {
                        InputDialogVisibility = true;
                    }));
            }
        }
        private RelayCommand cancelNewCommand;
        public RelayCommand CancelNewCommand
        {
            get
            {
                return cancelNewCommand ??
                    (cancelNewCommand = new RelayCommand(obj =>
                    {
                        InputDialogVisibility = false;
                    }));
            }
        }
        private RelayCommand confirmNewCommand;
        public RelayCommand ConfirmNewCommand
        {
            get
            {
                return confirmNewCommand ??
                    (confirmNewCommand = new RelayCommand(obj =>
                    {
                        AddData(obj.ToString());
                        InputDialogVisibility = false;
                    }));
            }
        }

        public void AddData(IEnumerable<string> paths)
        {
            foreach (string file in paths)
            {
                DataClass d = new DataClass();
                if (Directory.Exists(file))
                {
                    d.DocCode = new DirectoryInfo(file).Name;
                    d.Size = PdfProcessing.ProcessFolder(file, Recursive);

                }
                else if (File.Exists(file))
                {
                    d.DocCode = System.IO.Path.GetFileNameWithoutExtension(file);
                    d.Size = PdfProcessing.ProcessFile(file);
                }
                d.Link = System.IO.Path.GetDirectoryName(file);

                Data.Add(d);
            }
        }
        public void AddData(string docCode)
        {
            DataClass d = new DataClass();
            d.DocCode = docCode;
            Data.Add(d);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool objectCh = false;
        public bool ObjectCh
        {
            get { return objectCh; }
            set
            {
                if (objectCh != value)
                {
                    objectCh = value;
                    OnPropertyChanged("ObjectCh");
                }
            }
        }
        private bool subsCh = false;
        public bool SubsCh
        {
            get { return subsCh; }
            set
            {
                if (subsCh != value)
                {
                    subsCh = value;
                    OnPropertyChanged("SubsCh");
                }
            }
        }
        private bool tasksCh = false;
        public bool TasksCh
        {
            get { return tasksCh; }
            set
            {
                if (tasksCh != value)
                {
                    tasksCh = value;
                    OnPropertyChanged("TasksCh");
                }
            }
        }
        private bool publishCh = false;
        public bool PublishCh
        {
            get { return publishCh; }
            set
            {
                if (publishCh != value)
                {
                    publishCh = value;
                    OnPropertyChanged("PublishCh");
                }
            }
        }
        private bool correctCh = false;
        public bool CorrectCh
        {
            get { return correctCh; }
            set
            {
                if (correctCh != value)
                {
                    correctCh = value;
                    OnPropertyChanged("CorrectCh");
                }
            }
        }
        private bool nroCh = false;
        public bool NroCh
        {
            get { return nroCh; }
            set
            {
                if (nroCh != value)
                {
                    nroCh = value;
                    OnPropertyChanged("NroCh");
                }
            }
        }
        private bool nrcCh = false;
        public bool NrcCh
        {
            get { return nrcCh; }
            set
            {
                if (nrcCh != value)
                {
                    nrcCh = value;
                    OnPropertyChanged("NrcCh");
                }
            }
        }


        private bool CreateCommandEnabler()
        {
            bool settingsCh = User != null && User != "" && // Simple checks
                               Group != null && Group != "" &&
                               Phone != null && Phone != "";
            bool emptyCh = Data.Count != 0;

            var vtor_v = (PublishType == Variables.Instance.PublishTypes[1]);
            var cor = (Corrections != null && Corrections != "");
            var sub = (Subs != Variables.Instance.Subs[0]);

            CorrectCh = ((!cor && sub) || (!sub && (vtor_v == cor))) &&
                               ((cor == data.Any(d => { return d.SizeCor.Formats != 0; })) ||
                               (!cor == data.Any(d => { return d.SizeCor.Formats == 0; })));

            foreach (var d in Data)
                d.UpdateCor(cor);

            return settingsCh && emptyCh && objectCh && subsCh && tasksCh && publishCh && correctCh;
        }
    }

    class DataClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        
        private string docCode;
        public string DocCode
        {
            get { return docCode; }
            set
            {
                if (docCode != value)
                {
                    docCode = value;
                    OnPropertyChanged("DocCode");
                }
            }
        }
        private string link;
        public string Link
        {
            get { return link; }
            set
            {
                if (link != value)
                {
                    link = value;
                    OnPropertyChanged("Link");
                }
            }
        }
        private PdfFormat size = new PdfFormat();
        public PdfFormat Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    OnPropertyChanged("Size");
                }
            }
        }
        private PdfFormat sizeCor = new PdfFormat();
        public PdfFormat SizeCor
        {
            get { return sizeCor; }
            set
            {
                if (sizeCor != value)
                {
                    sizeCor = value;
                    OnPropertyChanged("SizeCor");
                }
            }
        }

        private bool correctCh = false;
        public bool CorrectCh
        {
            get { return correctCh; }
            set
            {
                if (correctCh != value)
                {
                    correctCh = value;
                    OnPropertyChanged("CorrectCh");
                }
            }
        }

        public void UpdateCor(bool need_corrections)
        {
            CorrectCh = need_corrections == (SizeCor.Formats != 0);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SizeAdd(PdfFormat p)
        {
            if (Size != null)
            {
                Size += p;
            }
            else
            {
                Size = p;
            }
        }
        public void SizeCorAdd(PdfFormat p)
        {
            if (SizeCor != null)
            {
                SizeCor += p;
            }
            else
            {
                SizeCor = p;
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
