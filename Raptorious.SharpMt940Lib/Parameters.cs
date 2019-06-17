using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// Codes for TransactionDetails fields
    /// </summary>
    public enum TransactionDetail { 
        /// <summary>
        /// customer account: IBAN or RNB
        /// </summary>
        Account, 
        /// <summary>
        /// customer name, address
        /// </summary>
        Name, 
        /// <summary>
        /// transaction description
        /// </summary>
        Description, 
        /// <summary>
        /// empty field - for unsopported field when 
        /// </summary>
        Empty 
    };

    /// <summary>
    /// Additional parameters for mt940
    /// </summary>
    /// <remarks>
    /// Field :86: contains also many additional data. 
    /// 1) Sometimes it includes few datas with constant separator
    /// :86:/EREF/1234567890ABCD/BENM/NAME/XYZ Corporation/REMI/PURCHASE OF GOODS/ISDT/2012-12-30
    /// 
    /// When bank uses constant separator "/"  use:
    /// mT940Params.AddCodeFor(MT940TrDetail.Description, "/");
    /// mT940Params.AddCodeFor(MT940TrDetail.Account, "/");
    /// mT940Params.AddCodeFor(MT940TrDetail.Empty, "/");
    /// mT940Params.AddCodeFor(MT940TrDetail.Name, "/");
    /// 
    /// 2) Sometimes it includes few datas with unique prefix
    /// :86:034~00XX20PRZELEW~20FA 2401 11/2015 ZAKUP~21LICENCJI PROGRAMU X~22zapata za fakturę~2222/11/2015~24~25~2821131014120510362270000000~3014105212~310010362230000000 ~32Customer name~38PL45101412160610362260000000
    /// 
    /// When bank uses prefixes use:
    /// mT940Params.AddCodeFor(MT940TrDetail.Account, "~38");
    /// mT940Params.AddCodeFor(MT940TrDetail.Name, "~32");
    /// mT940Params.AddCodeFor(MT940TrDetail.Description, "~22");
    /// </remarks>
    public class Parameters
    {
        /// <summary>
        /// Codes for any MT940TrDetail
        /// </summary>
        public Dictionary<TransactionDetail, string[]> Codes { get; } = new Dictionary<TransactionDetail, string[]>();

        /// <summary>
        /// Set default values
        /// </summary>
        public Parameters() 
        {
            ClearAccount = false;
        }

        /// <summary>
        /// Return clear Account identification (only alphanumeric characters)
        /// </summary>
        public bool ClearAccount { get; set; }

        /// <summary>
        /// determine max length value for every codes of TransactionDetails
        /// if MaxLineLength > 0 then if any MT940TrDetail has few codes then if value.Length > MaxLineLength then values will be concatenate without space
        /// </summary>
        public int MaxLineLength { get; set; }

        /// <summary>
        /// Insert code for TransactionDetails (geting :86: details)
        /// </summary>
        /// <param name="detailField"></param>
        /// <param name="codes"></param>
        public void AddCodeFor(TransactionDetail detailField, params string[] codes) 
        {
            if (codes == null || codes.Length == 0) 
            {
                throw new ArgumentException("codes can not be empty", nameof(codes));
            }

            foreach (var item in codes)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new ArgumentException("code can not be empty string", nameof(codes));
                }
            }
            Codes.Add(detailField, codes);
        }

        /// <summary>
        /// Check is all codes (separators) are the same
        /// </summary>
        /// <returns></returns>
        public bool ConstantCode() 
        {
            bool constantCode = true;
            string tempCode = "";
            foreach (var item in Codes)
            {
                foreach (string code in item.Value)
                {
                    if (string.IsNullOrEmpty(tempCode))
                    {
                        tempCode = code;
                    }
                    if (tempCode != code)
                    {
                        constantCode = false;
                        break;
                    }
                }
                if (!constantCode)
                {
                    break;
                }
            }
            return constantCode;
        }
    }
}
