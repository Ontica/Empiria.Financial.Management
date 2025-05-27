/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Data Layer                              *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Data Service                            *
*  Type     : FinancialProjectDataService                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial projects.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.Projects.Data {

  /// <summary>Provides data access services for financial projects.</summary>
  static internal class FinancialProjectDataService {

    static internal void CleanProject(FinancialProject project) {
      if (project.IsEmptyInstance) {
        return;
      }
      var sql = "UPDATE FMS_PROJECTS " +
                $"SET PRJ_NAME = '{EmpiriaString.Clean(project.Name).Replace("'", "''")}', " +
                $"PRJ_KEYWORDS = '{project.Keywords}' " +
                $"WHERE PRJ_ID = {project.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal FixedList<FinancialProject> SearchProjects(string keywords) {

      keywords = keywords ?? string.Empty;

      var filter = SearchExpression.ParseOrLikeKeywords("PRJ_KEYWORDS", keywords);
      if (filter.Length != 0) {
        filter += " AND";
      }
      var sql = "SELECT * FROM FMS_PROJECTS " +
                $"WHERE {filter} " +
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


    internal static void WriteProject(FinancialProject o, IIdentifiable subprogram, string extensionData) {
      var op = DataOperation.Parse("write_FMS_Project",
         o.Id, o.UID, o.FinancialProjectType.Id, subprogram.Id, o.Category.Id, o.ProjectNo, o.Name,
         o.BaseOrgUnit.Id, o.Identifiers, o.Tags, extensionData, o.Keywords, o.Parent.Id,
         o.StartDate, o.EndDate, o.Id, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class FinancialProjectDataService

}  // namespace Empiria.Financial.Projects.Data
