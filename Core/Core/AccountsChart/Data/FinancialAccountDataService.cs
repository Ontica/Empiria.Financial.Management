/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Account                                    Component : Data Layer                              *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Data Service                            *
*  Type     : AccountDataService                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial accounts.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.Data {

  /// <summary>Provides data access services for financial accounts.</summary>
  static internal class FinancialAccountDataService {

      static internal FixedList<FinancialAccount> SearchAccount(string keyewords) {
      
      var sql = "SELECT * FROM FMS_ACCOUNTS " +
               $"WHERE ACCT_KEYWORDS LIKE '%{keyewords}%'" + " AND " +
               $"ACCT_STATUS <> 'X' AND ACCT_TYPE_ID = 3216";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialAccount>(op);
    }


    static internal FixedList<FinancialAccount> SearchAccount(string filter, string sortBy) {
      
      var sql = "SELECT * FROM FMS_ACCOUNTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialAccount>(dataOperation);
    }

    internal static void WriteAccount(FinancialAccount o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Account",
         o.Id, o.UID, o.GetEmpiriaType().Id, o.StandardAccount.Id, o.Organization.Id, 
         o.OrganizationUnit.Id, o.Party.Id, o.Project.Id, o.LedgerId, o.AcctNo, o.Name, 
         o.Identifiers, o.Tags, o.Attributes, o.FinancialData, o.ConfigData, extensionData,
         o.Keywords, o.ParentId, o.StartDate, o.EndDate, o.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class AccountDataService

}  // namespace Empiria.Financial.Data
