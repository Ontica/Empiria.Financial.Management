/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Output DTO                              *
*  Type     : BudgetTypeForEditionDto                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return budget types for transactions edition.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Adapters;
using Empiria.StateEnums;

namespace Empiria.Budgeting.Transactions.Adapters {

  /// <summary>Output DTO used to return budget types for transactions edition.</summary>
  public class BudgetTypeForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public bool Multiyear {
      get; internal set;
    }

    public FixedList<BudgetForEditionDto> Budgets {
      get; internal set;
    }

  }  // class BudgetTypeForEditionDto



  /// <summary>Output DTO used to return budgets for transactions edition.</summary>
  public class BudgetForEditionDto : BudgetDto {

    public FixedList<BudgetSegmentTypeDto> SegmentTypes {
      get; internal set;
    }

    public FixedList<TransactionTypeForEditionDto> TransactionTypes {
      get; internal set;
    }

  }  // class BudgetForEditionDto


  /// <summary>Output DTO used to return budgets transaction types for edition.</summary>
  public class TransactionTypeForEditionDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<NamedEntityDto> OperationSources {
      get; internal set;
    }

    public FixedList<NamedEntityDto> RelatedDocumentTypes {
      get; internal set;
    }

    public TransactionTypeEntriesRulesDto EntriesRules {
      get; internal set;
    }

  }  // class TransactionTypeForEditionDto



  /// <summary>Output DTO used to return a budget transaction entries's rule.</summary>
  public class TransactionTypeEntriesRulesDto {

    public FixedList<NamedEntityDto> BalanceColumns {
      get; internal set;
    }

    public ThreeStateValue SelectProduct {
      get; internal set;
    }

    public FixedList<int> Years {
      get; internal set;
    }

  }  // class TransactionTypeEntriesRulesDto

}  //namespace Empiria.Budgeting.Transactions.Adapters
