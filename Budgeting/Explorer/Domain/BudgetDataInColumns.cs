/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Explorer                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Explorer.dll             Pattern   : Value type                              *
*  Type     : BudgetDataInColumns                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Value type that holds budget data in columns.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Budgeting.Explorer {

  /// <summary>Value type that holds budget data in columns.</summary>
  internal class BudgetDataInColumns {

    [DataField("BUDGET_ID")]
    internal Budget Budget {
      get; private set;
    }

    [DataField("YEAR")]
    internal int Year {
      get; private set;
    }

    [DataField("MONTH")]
    internal int Month {
      get; private set;
    }

    [DataField("BUDGET_ACCT_ID")]
    internal BudgetAccount BudgetAccount {
      get; private set;
    }

    [DataField("CURRENCY_ID")]
    internal Currency Currency {
      get; private set;
    }

    [DataField("PLANNED")]
    internal decimal Planned {
      get; private set;
    }

    [DataField("AUTHORIZED")]
    internal decimal Authorized {
      get; private set;
    }

    [DataField("EXPANDED")]
    internal decimal Expanded {
      get; private set;
    }

    [DataField("REDUCED")]
    internal decimal Reduced {
      get; private set;
    }

    [DataField("MODIFIED")]
    internal decimal Modified {
      get; private set;
    }

    [DataField("AVAILABLE")]
    internal decimal Available {
      get; private set;
    }

    [DataField("COMMITED")]
    internal decimal Commited {
      get; private set;
    }

    [DataField("TOPAY")]
    internal decimal ToPay {
      get; private set;
    }

    [DataField("EXERCISED")]
    internal decimal Excercised {
      get; private set;
    }

    [DataField("TOEXERCISE")]
    internal decimal ToExercise {
      get; private set;
    }

  }  // class BudgetDataInColumns

}  // namespace Empiria.Budgeting.Explorer
