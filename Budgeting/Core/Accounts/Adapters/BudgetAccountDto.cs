/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Output DTO                              *
*  Type     : BudgetAccountDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for budget accounts.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  public class BudgetAccountDto {

    public string UID {
      get; internal set;
    }

    public string BaseSegmentUID {
      get; internal set;
    }

    public string Code {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public NamedEntityDto Type {
      get; internal set;
    }

    public NamedEntityDto OrganizationalUnit {
      get; internal set;
    }

    public bool IsAssigned {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class BudgetAccountDto

}  // namespace Empiria.Budgeting.Adapters
