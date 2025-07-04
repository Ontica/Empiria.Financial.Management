﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Data Layer                              *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Data Service                            *
*  Type     : FinancialProjectDataService                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial projects.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

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


    static internal string GetNextProjectNo() {
      string sql = "SELECT MAX(PRJ_NO) " +
                   "FROM FMS_PROJECTS " +
                   $"WHERE LENGTH(PRJ_NO) = 5 AND " +
                   $"PRJ_STATUS <> 'X'";

      string lastUniqueID = DataReader.GetScalar(DataOperation.Parse(sql), string.Empty);

      if (lastUniqueID.Length != 0) {

        int consecutive = int.Parse(lastUniqueID) + 1;

        return $"{consecutive:00000}";

      } else {
        return "00001";
      }
    }


    internal static List<FinancialAccount> GetProjectAccounts(FinancialProject project) {
      if (project.IsEmptyInstance) {
        return new List<FinancialAccount>();
      }

      var sql = "SELECT * FROM FMS_ACCOUNTS " +
          $"WHERE ACCT_PROJECT_ID = {project.Id} AND " +
          $"ACCT_STATUS <> 'X' " +
          $"ORDER BY ACCT_NUMBER";


      var op = DataOperation.Parse(sql);

      return DataReader.GetList<FinancialAccount>(op);
    }


    static internal FixedList<FinancialProject> SearchProjects(string keywords) {

      keywords = keywords ?? string.Empty;

      var filter = SearchExpression.ParseOrLikeKeywords("PRJ_KEYWORDS", keywords);
      if (filter.Length != 0) {
        filter += " AND";
      }
      var sql = "SELECT * FROM FMS_PROJECTS " +
                $"WHERE {filter} " +
                $"PRJ_STATUS <> 'X' " +
                $"ORDER BY PRJ_NO";

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
         o.Id, o.UID, o.FinancialProjectType.Id, o.Subprogram.Id, o.Category.Id, o.ProjectNo, o.Name,
         o.BaseOrgUnit.Id, o.Description, o.Justification, o.Assignee.Id,
         EmpiriaString.Tagging(o.Identifiers), EmpiriaString.Tagging(o.Tags), extensionData,
         o.ProjectGoals.ToJsonString(), o.RecordingTime, o.RecordedBy.Id,
         o.AuthorizationTime, o.AuthorizedBy.Id, o.Keywords, o.Parent.Id, o.HistoricId,
         o.StartDate, o.EndDate, o.PostedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class FinancialProjectDataService

}  // namespace Empiria.Financial.Projects.Data
