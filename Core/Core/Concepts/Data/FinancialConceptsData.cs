/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Data Layer                              *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Data Service                            *
*  Type     : FinancialConceptsData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access services for financial concepts.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Financial.Concepts.Data {

  /// <summary>Provides data access services for financial concepts.</summary>
  static internal class FinancialConceptsData {

    static internal FixedList<FinancialConcept> GetConcepts(FinancialConceptGroup group) {
      var sql = "SELECT * FROM FMS_CONCEPTS " +
               $"WHERE CPT_GROUP_ID = {group.Id} AND " +
               $"CPT_STATUS <> 'X' " +
               $"ORDER BY CPT_NO";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FinancialConcept>(op);
    }

  }  // class FinancialConceptsData

}  // namespace Empiria.Financial.Concepts.Data
