/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects                                   Component : Data Layer                              *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Data Service                            *
*  Type     : ProjectDataService                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial projects.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;

namespace Empiria.Financial.Data {

  /// <summary>Provides data access services for financial projects.</summary>
  static internal class ProjectDataService {


      static internal FixedList<FinancialProject> SearchProjects(string keyewords) {
      
      var sql = "SELECT * FROM FMS_PROJECTS " +
               $"WHERE PRJ_KEYWORDS LIKE '% {keyewords} %'" + " AND " +
               $"PRJ_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialProject>(op);
    }

    static internal FixedList<FinancialProject> SearchProjects(string filter, string sortBy) {
      
      var sql = "SELECT * FROM FMS_PROJECTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialProject>(dataOperation);
    }

    internal static void WriteProject(FinancialProject o, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Project",
         o.Id, o.UID, o.ProjectTypeId, o.StandarAccount.Id, o.CategoryId, o.PrjNo, o.Name,
         o.OrganizationUnit.Id, o.Identifiers, o.Tags, extensionData, o.Keywords, o.ParentId,
         o.StartDate, o.EndDate, o.HistoricId, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }
  }  // class ProjectDataService

}  // namespace Empiria.Financial.Data
