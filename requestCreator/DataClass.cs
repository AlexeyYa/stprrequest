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

        private string savePath;
        public string SavePath
        {
            get { return savePath; }
            set
            {
                if (savePath != value)
                {
                    if (value[value.Length - 1] != '\\'){
                        value += '\\';
                    }
                    savePath = value;
                    OnPropertyChanged("SavePath");
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
        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                    (createCommand = new RelayCommand(obj =>
                    {
                        if (!DocxModule.Create(this, SavePath))
                        {
                            MessageBoxResult messageBox = MessageBox.Show("Не удалось создать файл", "Ошибка");
                        }
                    },
                    obj => { return User != null && Group != null && Object != null && Phone != null && Subs != String.Empty
                                    && Tasks != null && Data.Count != 0 && PublishType != null && SavePath != null; }));
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
        private PdfFormat size;
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
        private PdfFormat sizeCor;
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
