/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Input Fields DTO                        *
*  Type     : CashFlowProjectionFields                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update cash flow projections.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;

using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.CashFlow.Projections {

  /// <summary>Input fields DTO used to create and update cash flow projections.</summary>
  public class CashFlowProjectionFields {

    public string PlanUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string BasePartyUID {
      get; set;
    } = string.Empty;


    public string BaseProjectUID {
      get; set;
    } = string.Empty;


    public string BaseAccountUID {
      get; set;
    } = string.Empty;


    public string OperationSourceUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public DateTime ApplicationDate {
      get; set;
    } = ExecutionServer.DateMaxValue;

  }  // class CashFlowProjectionFields



  /// <summary>Extension methods for CashFlowProjectionFields type.</summary>
  static internal class CashFlowProjectionFieldsExtensions {

    static internal void EnsureValid(this CashFlowProjectionFields fields) {
      fields.Description = EmpiriaString.Clean(fields.Description);
      fields.Justification = EmpiriaString.Clean(fields.Justification);

      if (fields.PlanUID.Length != 0) {
        _ = CashFlowPlan.Parse(fields.PlanUID);
      }

      if (fields.CategoryUID.Length != 0) {
        _ = CashFlowProjectionCategory.Parse(fields.CategoryUID);
      }

      if (fields.BasePartyUID.Length != 0) {
        _ = Party.Parse(fields.BasePartyUID);
      }

      if (fields.BaseProjectUID.Length != 0) {
        _ = FinancialProject.Parse(fields.BaseProjectUID);
      }

      if (fields.BaseAccountUID.Length != 0) {
        _ = FinancialAccount.Parse(fields.BaseAccountUID);
      }

      if (fields.OperationSourceUID.Length != 0) {
        _ = OperationSource.Parse(fields.OperationSourceUID);
      }

    }

  }  // class CashFlowProjectionFieldsExtensions

}  // namespace Empiria.CashFlow.Projections
