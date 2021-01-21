/*
* Copyright (c) 2012 Jaco Adriaansen
* This code is distributed under the MIT (for details please see license.txt)
*/

using Raptorious.SharpMt940Lib.Mt940Format;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    ///
    /// </summary>
    public static class Mt940Parser
    {
        /// <summary>
        /// Opens the given file, reads it and returns a collection of CustomerStatementMessages using the current culture.
        /// </summary>
        /// <param name="file">File to read.</param>
        /// <param name="format">Bank specific format of this file.</param>
        /// <param name="mt940Params">Additional mt940 parameters</param>
        /// <returns>Returns a collection of customer statement messages, populated by the data of the given file.</returns>
        [Obsolete("Please use the Parse method with CultureInfo")]
        public static ICollection<CustomerStatementMessage> Parse(IMt940Format format, string file, Parameters mt940Params = null)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file can not be empty", file);
            }

            return Parse(format, file, CultureInfo.CurrentCulture, mt940Params);
        }

        /// <summary>
        /// Opens the given file, reads it and returns a collection of CustomerStatementMessages.
        /// </summary>
        /// <param name="file">File to read.</param>
        /// <param name="format">Bank specific format of this file.</param>
        /// <param name="cultureInfo">Specifies the culture to use</param>
        /// <param name="mt940Parameters">Additional mt940 parameters</param>
        /// <returns>Returns a collection of customer statement messages, populated by the data of the given file.</returns>
        public static ICollection<CustomerStatementMessage> Parse(IMt940Format format, string file, CultureInfo cultureInfo, Parameters mt940Parameters = null)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file can not be empty");
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }


            if (!File.Exists(file))
            {
                throw new FileNotFoundException("Can not find file.", file);
            }

            using (StreamReader reader = new StreamReader(File.OpenRead(file)))
            {
                return Parse(format, reader, cultureInfo, mt940Parameters);
            }
        }

        /// <summary>
        /// Reads the given string to the end and parses the data to Customer Statement Messages.
        /// </summary>
        /// <param name="fileStream">Filestream to read.</param>
        /// <param name="format">Bank specific format of this file.</param>
        /// <param name="mt940Params">Additional mt940 parameters</param>
        /// <returns></returns>
        [Obsolete("Please use the Parse method with CultureInfo")]
        public static ICollection<CustomerStatementMessage> Parse(IMt940Format format, TextReader fileStream, Parameters mt940Params = null)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            return Parse(format, fileStream, CultureInfo.CurrentCulture, mt940Params);
        }

        /// <summary>
        /// Reads the given string to the end and parses the data to Customer Statement Messages.
        /// </summary>
        /// <param name="fileStream">Filestream to read.</param>
        /// <param name="format">Bank specific format of this file.</param>
        /// <param name="cultureInfo">Specifies the culture information to use</param>
        /// <param name="mt940Parameters">Additional mt940 parameters</param>
        /// <returns></returns>
        public static ICollection<CustomerStatementMessage> Parse(IMt940Format format, TextReader fileStream, CultureInfo cultureInfo, Parameters mt940Parameters = null)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            var completeFile = fileStream.ReadToEnd();
            return ParseData(format, completeFile, cultureInfo, mt940Parameters);
        }

        /// <summary>
        /// Reads the given string to the end and parses the data to Customer Statement Messages using the current culture
        /// </summary>
        /// <param name="format">Bank specific format of this file.</param>
        /// <param name="data">String containing the MT940 file.</param>
        /// <param name="mt940Params">Additional mt940 parameters</param>
        /// <returns></returns>
        [Obsolete("Please use the Parse method with CultureInfo")]
        public static ICollection<CustomerStatementMessage> ParseData(IMt940Format format, string data, Parameters mt940Params = null)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data can not be empty", nameof(data));
            }

            if (mt940Params == null)
            {
                // default values
                mt940Params = new Parameters();
            }

            return ParseData(format, data, CultureInfo.CurrentCulture, mt940Params);
        }

        /// <summary>
        /// Reads the given string to the end and parses the data to Customer Statement Messages
        /// </summary>
        /// <param name="format"></param>
        /// <param name="data"></param>
        /// <param name="cultureInfo">Specifies the culture information to use</param>
        /// <param name="mt940Parameters">Additional mt940 parameters</param>
        /// <returns></returns>
        public static ICollection<CustomerStatementMessage> ParseData(IMt940Format format, string data, CultureInfo cultureInfo, Parameters mt940Parameters = null)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("data can not be empty", nameof(data));
            }

            if (mt940Parameters == null)
            {
                // default values
                mt940Parameters = new Parameters();
            }

            var listData = CreateStringTransactions(format, data);
            return CreateObjectTransactions(format, listData, cultureInfo, mt940Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">IMt940Format implementation</param>
        /// <param name="data">A collection of string arrays formatted by CreateStringTransactions()</param>
        /// <param name="cultureInfo">The culture to use</param>
        /// <param name="mt940Params">Additional mt940 parameters</param>
        /// <see cref="CreateStringTransactions"></see>
        /// <returns></returns>
        /// TODO: This method is way to complex. It should be simplified.
        private static ICollection<CustomerStatementMessage> CreateObjectTransactions(
            IMt940Format format,
            ICollection<string[]> data,
            CultureInfo cultureInfo,
            Parameters mt940Params)
        {
            // Create a new list.
            var customerStatementList = new List<CustomerStatementMessage>();


            // For each string collection of commands.
            foreach (string[] line in data)
            {
                int transactionPointer = 0; // Start of the transaction.

                // Skip the header (for some reason)
                transactionPointer += format.Header.LineCount; // SWIFT HEADER.

                var transaction = default(Transaction); // Set transaction to its default (null).
                var customerStatementMessage = new CustomerStatementMessage();

                // ReSharper disable AccessToModifiedClosure
                void addAndNullTransactionIfPresent()
                {
                    if (customerStatementMessage != null && transaction != null)
                        customerStatementMessage.Transactions.Add(transaction);
                    transaction = null;
                }
                // ReSharper restore AccessToModifiedClosure

                // Loop through the array.
                for (; transactionPointer < line.Length; transactionPointer++)
                {
                    // Set transactionLine to the current line.
                    string transactionLine = line[transactionPointer];

                    // Skip if null, CreateObjectTransactions kinda leaves a mess.
                    if (transactionLine != null)
                    {
                        // Get the command number.
                        var tag = transactionLine.Substring(transactionLine.IndexOf(':'), transactionLine.IndexOf(':', 1) + 1);

                        // Get the command data.
                        var transactionData = transactionLine.Substring(tag.Length);

                        // Fill the object the right data.
                        switch (tag)
                        {
                            case ":20:":
                                customerStatementMessage = customerStatementMessage.SetTransactionReference(transactionData);
                                break;
                            case ":21:":
                                customerStatementMessage = customerStatementMessage.SetRelatedMessage(transactionData);
                                break;
                            case ":25:":
                                customerStatementMessage = customerStatementMessage.SetAccount(transactionData, mt940Params.ClearAccount);
                                break;
                            case ":28:":
                            case ":28C:":
                                customerStatementMessage.SetSequenceNumber(transactionData, cultureInfo);
                                break;
                            case ":60m:":
                            case ":60F:":
                            case ":60M:":
                                customerStatementMessage = customerStatementMessage.SetOpeningBalance(new TransactionBalance(transactionData, cultureInfo));
                                break;
                            case ":61:":
                                transaction = new Transaction(transactionData, customerStatementMessage.OpeningBalance.Currency, cultureInfo);
                                addAndNullTransactionIfPresent();
                                break;
                            case ":86:":
                                /* 
                                * If the previous line was a 61 (ie, we have a transaction), the 'Information to Account Owner'
                                * applies to the transaction, otherwise it applies to the whole message.
                                */
                                if (transaction == null)
                                    customerStatementMessage = customerStatementMessage.SetDescription(transactionData);
                                else
                                {
                                    transaction.Description = transactionData;
                                }
                                addAndNullTransactionIfPresent();
                                break;
                            case ":62F:":
                                addAndNullTransactionIfPresent();
                                customerStatementMessage = customerStatementMessage.SetClosingBalance(new TransactionBalance(transactionData, cultureInfo));
                                break;
                            case ":62m:":
                            case ":62M:":
                                customerStatementMessage = customerStatementMessage.SetClosingBalance(new TransactionBalance(transactionData, cultureInfo));
                                break;
                            case ":64:":
                                addAndNullTransactionIfPresent();
                                customerStatementMessage = customerStatementMessage.SetClosingAvailableBalance(new TransactionBalance(transactionData, cultureInfo));
                                break;
                            case ":65:":
                                customerStatementMessage = customerStatementMessage.SetForwardAvailableBalance(new TransactionBalance(transactionData, cultureInfo));
                                break;
                        }
                    }
                }

                customerStatementList.Add(customerStatementMessage);
            }

            // geting :86: details
            if (mt940Params.Codes.Count > 0)
            {
                var constantCode = mt940Params.ConstantCode();

                foreach (var item in customerStatementList)
                {
                    foreach (var tr in item.Transactions)
                    {
                        Parse86Details(tr, mt940Params, constantCode);
                    }
                }
            }

            return customerStatementList;
        }

        private static void Parse86Details(Transaction tr, Parameters mt940Params, bool constantCode)
        {
            if (constantCode)
            {
                throw new NotImplementedException("to do...");
            }
            else
            {
                string data = tr.Description;
                char separator = mt940Params.Codes.First().Value[0][0];
                foreach (var item in mt940Params.Codes)
                {
                    string[] _codes = item.Value;
                    string value = "";
                    foreach (var cs in _codes)
                    {
                        var startIndex = data.IndexOf(cs, StringComparison.CurrentCulture);
                        if (startIndex > -1)
                        {
                            startIndex += cs.Length;
                            var endIndex = data.IndexOf(separator, startIndex);
                            var line = data.Substring(startIndex, endIndex - startIndex).Trim();
                            if (mt940Params.MaxLineLength > 0 && line.Length >= mt940Params.MaxLineLength)
                                value += line;
                            else
                                value += line + " ";
                        }
                    }
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        switch (item.Key)
                        {
                            case TransactionDetail.Account:
                                tr.Details.Account = value;
                                break;
                            case TransactionDetail.Name:
                                tr.Details.Name = value;
                                break;
                            case TransactionDetail.Description:
                                tr.Details.Description = value;
                                break;
                            case TransactionDetail.Empty:
                            default:
                                break;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// This method accepts mt940 data file given as a string. The string 
        /// is split by Environment.NewLine as each line contains a command.
        /// 
        /// Every line that starts with a ':' is a mt940 'command'. Lines that 
        /// does not start with a ':' belongs to the previous command. 
        /// 
        /// The method returns a collection of string arrays. Every item in 
        /// the collection is a mt940 message. 
        /// </summary>
        /// <param name="data">A string of MT940 data to parse.</param>
        /// <param name="format">Specifies the bank specific format</param>
        /// <returns>A collection :)</returns>
        private static ICollection<string[]> CreateStringTransactions(IMt940Format format, string data)
        {
            // Split on the new line seperator. In a MT940 messsage, every command is on a seperate line.
            // Assumption is made it is in the same format as the enviroments new line.
            var tokenized = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Create  an empty list of string arrays.
            var transactions = new List<string[]>();


            // Offset pointer, starts a the first line (zero based index).
            int pointer = 0;

            // Loop trough the entire file?
            while (pointer < tokenized.Length)
            {
                // Seperator, this is the Trailer! We split messages based on trailer! - Right, check.
                var trailerIndex = Array.IndexOf(tokenized, format.Trailer.Data, pointer);

                // When we found a trailer.. then..
                if (trailerIndex >= 0)
                {
                    // Create a new array the holds the correct number of elements.
                    string[] currentTransaction = new string[trailerIndex - pointer];

                    // Copy the data from the source array to our current transaction.
                    Array.Copy(tokenized, pointer, currentTransaction, 0, currentTransaction.Length);

                    // Walk trough the current message. Start at the current 
                    // index and stop at the separator.
                    for (int index = currentTransaction.Length - 1;
                         index > format.Header.LineCount;
                         index--)
                    {
                        // 
                        string transactionItem = currentTransaction[index];

                        System.Diagnostics.Debug.Assert(transactionItem != null);

                        // If the transactionItem doesn't start with : then the
                        // current line belongs to the previous one.
                        if (!transactionItem.StartsWith(":", StringComparison.Ordinal))
                        {
                            // Append ths current line to the previous line seperated by
                            // and NewLine.
                            currentTransaction[index - 1] += Environment.NewLine;
                            currentTransaction[index - 1] += transactionItem;

                            // Set the current item to null, it doesn't exist anymore.
                            currentTransaction[index] = null;
                        }
                    }

                    // Add the current transaction.
                    transactions.Add(currentTransaction);

                    // Next up!
                    pointer = (trailerIndex + 1);
                }
                else
                {
                    // Message doesn't contain a trailer. So it is invalid!
                    throw new InvalidDataException("Can not find trailer!");
                }
            }

            return transactions;
        }
    }
}
