/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information holder                      *
*  Type     : FinancialProjectGoals                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains demographic and financial goals data for a financial project.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Financial.Projects {

  /// <summary>Contains demographic and financial goals data for a financial project.</summary>
  public class FinancialProjectGoals {

    private readonly JsonObject _projectGoals;

    internal FinancialProjectGoals(JsonObject projectGoals) {

      _projectGoals = projectGoals;
    }

  }  // class FinancialProjectGoals

}  //namespace Empiria.Financial.Projects
