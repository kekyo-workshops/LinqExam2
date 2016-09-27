/////////////////////////////////////////////////////////////////////////////////////////////////
//
// TextFieldContext
// Copyright (c) 2016 Kouji Matsui (@kekyo2)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace LinqExam2
{
    public sealed class TextFieldContext : IEnumerable<string[]>
    {
        private readonly string path_;
        private readonly string separator_;
        private readonly Encoding encoding_;

        public TextFieldContext(string path, string separator = ",", Encoding encoding = null)
        {
            path_ = path;
            separator_ = separator;
            encoding_ = encoding;

            this.FieldNames = new string[0];
        }

        public string[] FieldNames
        {
            get;
            private set;
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            try
            {
                using (var stream = new FileStream(path_, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var parser = new TextFieldParser(stream, encoding_ ?? Encoding.UTF8, true, false))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.Delimiters = new[] { separator_ };
                        parser.HasFieldsEnclosedInQuotes = true;
                        parser.TrimWhiteSpace = true;

                        if (parser.EndOfData == false)
                        {
                            this.FieldNames = parser.ReadFields();

                            while (parser.EndOfData == false)
                            {
                                var fields = parser.ReadFields();
                                yield return fields;
                            }
                        }
                    }
                }
            }
            finally
            {
                this.FieldNames = new string[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
