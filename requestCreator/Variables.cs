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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace requestCreator
{
    public class Variables
    {
        public const string pathTemplate = @".\cfg\template.docx";
        const string pathObjects = @".\cfg\objects.txt";
        const string pathGroups = @".\cfg\groups.txt";
        const string pathSubs = @".\cfg\subs.txt";
        const string pathPublishTypes = @".\cfg\publishtype.txt";
        const string pathCorrections = @".\cfg\corrections.txt";

        private static Variables instance;
        public static Variables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Variables();
                }
                return instance;
            }
        }

        private List<string> objects = new List<string>(),
            groups = new List<string>(),
            subs = new List<string>(),
            publishTypes = new List<string>(),
            recieversList = new List<string>();

        private Dictionary<Tuple<int, string>, Dictionary<int, string>> corrections = new Dictionary<Tuple<int, string>, Dictionary<int, string>>();

        private string path, filename;
        public List<string> RecieversList
        {
            get
            {
                return recieversList;
            }
        }

        public List<string> Objects
        {
            get
            {
                return objects;
            }
        }
        public List<string> Groups
        {
            get
            {
                return groups;
            }
        }
        public List<string> Subs
        {
            get
            {
                return subs;
            }
        }
        public List<string> PublishTypes
        {
            get
            {
                return publishTypes;
            }
        }
        public Dictionary<Tuple<int, string>, Dictionary<int, string>> Corrections
        {
            get
            {
                return corrections;
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
        }
        public string Filename
        {
            get
            {
                return filename;
            }
        }

        public Variables()
        {
            LoadObjects();
            LoadGroups();
            LoadSubs();
            LoadPublishTypes();
            LoadCorrections();
            LoadConfig();
        }

        private void LoadObjects()
        {
            string[] objectsArray = File.ReadAllLines(pathObjects);
            foreach (string elem in objectsArray)
            {
                objects.Add(elem);
            }
        }
        private void LoadGroups()
        {
            string[] groupsArray = File.ReadAllLines(pathGroups);
            foreach (string g in groupsArray)
            {
                groups.Add(g);
            }
        }
        private void LoadSubs()
        {
            string[] subsArray = File.ReadAllLines(pathSubs);
            foreach (string sub in subsArray)
            {
                subs.Add(sub);
            }
        }
        private void LoadPublishTypes()
        {
            string[] publishArray = File.ReadAllLines(pathPublishTypes);
            foreach (string pub in publishArray)
            {
                publishTypes.Add(pub);
            }
        }
        private void LoadCorrections()
        {
            Tuple<int, string> tup = new Tuple<int, string>(0, "ERROR");

            string[] corrFile = File.ReadAllLines(pathCorrections);
            foreach (string line in corrFile)
            {
                if (line != null && line != "")
                {
                    if (line[0] != '-')
                    {
                        int idx = line.IndexOf('#');
                        tup = new Tuple<int, string>(Int32.Parse(line.Substring(0, idx)), line.Substring(idx + 1));
                        corrections[tup] = new Dictionary<int, string>();
                    }
                    else
                    {
                        int idx = line.IndexOf('#');
                        int cor_id = Int32.Parse(line.Substring(1, idx - 1));
                        corrections[tup][cor_id] = line.Substring(idx + 1);
                    }
                }
            }
        }
        public void LoadConfig()
        {
            try
            {
                string[] cfg = File.ReadAllLines(Properties.Settings.Default.ConfigPath);

                if (cfg.Length > 0)
                {
                    recieversList = new List<string>();

                    int status = -1; // 0 - Recievers; 1 - Folder; 2 - Filename
                    foreach (string line in cfg)
                    {
                        if (line == "Recievers:")
                            status = 0;
                        else if (line == "Folder:")
                            status = 1;
                        else if (line == "Filename:")
                            status = 2;
                        else
                            switch (status)
                            {
                                case 0:
                                    recieversList.Add(line);
                                    break;
                                case 1:
                                    path = line;
                                    break;
                                case 2:
                                    filename = line;
                                    break;
                            }
                    }
                }
                else
                {
                    MessageBoxResult messageBox = MessageBox.Show("Файл конфигурации пуст", "Ошибка");
                }
            }
            catch (Exception e)
            {
                MessageBoxResult messageBox = MessageBox.Show("Ошибка при загрузке конфигурации\n" + e, "Ошибка");
            }
        }
    }
}
