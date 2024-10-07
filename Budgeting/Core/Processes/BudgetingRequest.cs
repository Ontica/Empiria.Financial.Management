/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Procesess                           Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : BudgetingRequest                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a budgeting request.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.DataObjects;

using Empiria.Workflow.Requests;
using Empiria.Workflow.Requests.Adapters;

namespace Empiria.Budgeting.Processes {

  /// <summary>Represents a budgeting request.</summary>
  public class BudgetingRequest : Request {

    protected BudgetingRequest(RequestType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    #region Properties

    public Budget Budget {
      get {
        return base.ExtensionData.Get<Budget>("budgetId");
      }
      private set {
        base.ExtensionData.Set("budgetId", value.Id);
      }
    }

    public override FixedList<FieldValue> RequestTypeFields {
      get {
        return new List<FieldValue> {
          new FieldValue { Field = "budget", Value = Budget.UID }
        }.ToFixedList();
      }
    }

    #endregion Properties

    #region Methods

    protected override void Update(RequestFields fields) {
      base.Update(fields);

      this.Budget = Budget.Parse(fields.RequestTypeFields.Find(x => x.Field == "budget").Value);

      if (fields.Description.Length == 0) {
        this.Description = $"{RequestType.DisplayName} - {Budget.Year}";
      }
    }

    #endregion Methods

  }  // class BudgetingRequest

}  // namespace Empiria.Budgeting.Processes
