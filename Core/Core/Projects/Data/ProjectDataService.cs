/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Projects                                   Component : Data Layer                              *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Data Service                            *
*  Type     : ProjectDataService                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial projects.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.Data {

  /// <summary>Provides data access services for financial projects.</summary>
  static internal class ProjectDataService {


    static internal FixedList<Project> SearchProjects(string keyewords)  {
      var sql = "SELECT * FROM FMS_PROJECTS " +
               $"WHERE PRJ_KEYWORDS LIKE '% {keyewords} %'"  + " AND " +
               $"PRJ_STATUS <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Project>(op);
    }

    static internal FixedList<Project> SearchProjects(string filter, string sortBy) {
      var sql = "SELECT * FROM FMS_PROJECTS ";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Project>(dataOperation);
    }


  }  // class ProjectDataService

}  // namespace Empiria.Financial.Data
