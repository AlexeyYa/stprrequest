﻿/*

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

namespace requestCreator
{
    public class Variables
    {
        const string pathObjects = @".\cfg\objects.txt";
        const string pathGroups = @".\cfg\groups.txt";
        const string pathSubs = @".\cfg\subs.txt";
        const string pathPublishTypes = @".\cfg\publishtype.txt";

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

        private List<string> objects = new List<string>(), groups = new List<string>(),
            subs = new List<string>(), publishTypes = new List<string>();
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

        public Variables()
        {
            LoadObjects();
            LoadGroups();
            LoadSubs();
            LoadPublishTypes();
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
    }
}
