/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Value type                              *
*  Type     : BudgetDataInColumns                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Value type that holds budget data in columns.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Explorer {

  /// <summary>Value type that holds budget data in columns.</summary>
  public class BudgetDataInColumns {

    [DataField("BUDGET_ID")]
    public Budget Budget {
      get; private set;
    }

    [DataField("BUDGET_YEAR", ConvertFrom = typeof(long))]
    public int Year {
      get; private set;
    }

    [DataField("BUDGET_MONTH", ConvertFrom = typeof(long))]
    public int Month {
      get; private set;
    }

    public string MonthName {
      get {
        if (Month == 0) {
          return Year.ToString();
        }
        return EmpiriaString.MonthName(Month);
      }
    }


    [DataField("BUDGET_ACCT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }

    [DataField("PLANNED")]
    public decimal Planned {
      get; private set;
    }

    [DataField("AUTHORIZED")]
    public decimal Authorized {
      get; private set;
    }

    [DataField("EXPANDED")]
    public decimal Expanded {
      get; private set;
    }

    [DataField("REDUCED")]
    public decimal Reduced {
      get; private set;
    }

    [DataField("MODIFIED")]
    public decimal Modified {
      get; private set;
    }

    [DataField("REQUESTED")]
    public decimal Requested {
      get; private set;
    }

    [DataField("COMMITED")]
    public decimal Commited {
      get; private set;
    }

    [DataField("PROVISIONED")]
    public decimal Provisioned {
      get; private set;
    }

    [DataField("TOPAY")]
    public decimal ToPay {
      get; private set;
    }

    [DataField("EXERCISED")]
    public decimal Exercised {
      get; private set;
    }

    [DataField("TOEXERCISE")]
    public decimal ToExercise {
      get; private set;
    }

    [DataField("AVAILABLE")]
    public decimal Available {
      get; private set;
    }

  }  // class BudgetDataInColumns

}  // namespace Empiria.Budgeting.Explorer
