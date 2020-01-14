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
using Xceed.Words.NET;

namespace requestCreator
{
    class DocxModule
    {
        public static bool Create(ViewModel vm, string path)
        {
            if (vm.Data.Count == 0) { return false; }
            foreach (var data in vm.Data)
            {
                //try
                {
                    DocX document = DocX.Load(@".\cfg\template.docx");

                    document.ReplaceText("#DOCCODE#", data.DocCode);
                    document.ReplaceText("#FIO#", vm.User);
                    document.ReplaceText("#GROUP#", vm.Group);
                    document.ReplaceText("#OBJCODE#", vm.Object);
                    document.ReplaceText("#FGM#", vm.Tasks);
                    document.ReplaceText("#LINK#", data.Link);
                    document.ReplaceText("#PHONE#", vm.Phone);
                    document.ReplaceText("#SUBS#", vm.Subs);
                    document.ReplaceText("#COMMENTS#", vm.Comments);
                    document.ReplaceText("#PUBLISHTYPE#", vm.PublishType);

                    if (vm.EndDate != null)
                    {
                        document.ReplaceText("#DATE#", DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " Срок "
                            + vm.EndDate.Value.ToString("yyyy-MM-dd HH:mm"));
                    }
                    else
                    {
                        document.ReplaceText("#DATE#", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    }

                    document.ReplaceText("#ORIG#", vm.NumberOfOriginals.ToString());
                    document.ReplaceText("#COPY#", vm.NumberOfCopies.ToString());

                    if (data.Size != null)
                    {
                        document.ReplaceText("#SF#", data.Size.Formats.ToString());
                        document.ReplaceText("#SA4#", data.Size.A4.ToString());
                        document.ReplaceText("#SA3#", data.Size.A3.ToString());
                        document.ReplaceText("#SA2#", data.Size.A2.ToString());
                        document.ReplaceText("#SA1#", data.Size.A1.ToString());
                        document.ReplaceText("#SA0#", data.Size.A0.ToString());
                    }
                    else
                    {
                        document.ReplaceText("#SF#", "0");
                        document.ReplaceText("#SA4#", "0");
                        document.ReplaceText("#SA3#", "0");
                        document.ReplaceText("#SA2#", "0");
                        document.ReplaceText("#SA1#", "0");
                        document.ReplaceText("#SA0#", "0");
                    }

                    if (data.SizeCor != null)
                    {
                        document.ReplaceText("#CF#", data.SizeCor.Formats.ToString());
                        document.ReplaceText("#CA4#", data.SizeCor.A4.ToString());
                        document.ReplaceText("#CA3#", data.SizeCor.A3.ToString());
                        document.ReplaceText("#CA2#", data.SizeCor.A2.ToString());
                        document.ReplaceText("#CA1#", data.SizeCor.A1.ToString());
                        document.ReplaceText("#CA0#", data.SizeCor.A0.ToString());
                    }
                    else
                    {
                        document.ReplaceText("#CF#", "0");
                        document.ReplaceText("#CA4#", "0");
                        document.ReplaceText("#CA3#", "0");
                        document.ReplaceText("#CA2#", "0");
                        document.ReplaceText("#CA1#", "0");
                        document.ReplaceText("#CA0#", "0");
                    }

                    if (vm.Tasks.Contains("Сканирование") && data.Size != null)
                    {
                        document.ReplaceText("#EF#", data.Size.Formats.ToString());
                        document.ReplaceText("#EA4#", data.Size.A4.ToString());
                        document.ReplaceText("#EA3#", data.Size.A3.ToString());
                        document.ReplaceText("#EA2#", data.Size.A2.ToString());
                        document.ReplaceText("#EA1#", data.Size.A1.ToString());
                        document.ReplaceText("#EA0#", data.Size.A0.ToString());
                    }
                    else
                    {
                        document.ReplaceText("#EF#", "0");
                        document.ReplaceText("#EA4#", "0");
                        document.ReplaceText("#EA3#", "0");
                        document.ReplaceText("#EA2#", "0");
                        document.ReplaceText("#EA1#", "0");
                        document.ReplaceText("#EA0#", "0");
                    }

                    if (vm.Tasks.Contains("Сканирование") && data.SizeCor != null)
                    {
                        document.ReplaceText("#ECF#", data.SizeCor.Formats.ToString());
                        document.ReplaceText("#ECA4#", data.SizeCor.A4.ToString());
                        document.ReplaceText("#ECA3#", data.SizeCor.A3.ToString());
                        document.ReplaceText("#ECA2#", data.SizeCor.A2.ToString());
                        document.ReplaceText("#ECA1#", data.SizeCor.A1.ToString());
                        document.ReplaceText("#ECA0#", data.SizeCor.A0.ToString());
                    }
                    else
                    {
                        document.ReplaceText("#ECF#", "0");
                        document.ReplaceText("#ECA4#", "0");
                        document.ReplaceText("#ECA3#", "0");
                        document.ReplaceText("#ECA2#", "0");
                        document.ReplaceText("#ECA1#", "0");
                        document.ReplaceText("#ECA0#", "0");
                    }

                    for (int i = 1; i <= 6; i++)
                    {
                        for (int j = 1; j <= 5; j++)
                        {
                            if (vm.Corrections.Contains(i + "-" + j))
                            {
                                document.ReplaceText("#" + i + "-" + j + "#", "+");
                            }
                            else
                            {
                                document.ReplaceText("#" + i + "-" + j + "#", "");
                            }
                        }
                    }
                    if (vm.Corrections.Contains("7"))
                    {
                        document.ReplaceText("#7#", "+");
                    }
                    else
                    {
                        document.ReplaceText("#7#", "");
                    }

                    document.SaveAs(path + data.DocCode + @".docx");
                }
                //catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
