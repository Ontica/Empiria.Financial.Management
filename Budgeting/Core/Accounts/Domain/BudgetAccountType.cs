﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power type                              *
*  Type     : BudgetAccountType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget account.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Budgeting {

  /// <summary>Power type that describes a budget account.</summary>
  [Powertype(typeof(BudgetAccount))]
  public sealed class BudgetAccountType : Powertype {

    #region Constructors and parsers

    private BudgetAccountType() {
      // Empiria powertype types always have this constructor.
    }

    static public new BudgetAccountType Parse(int typeId) {
      return ObjectTypeInfo.Parse<BudgetAccountType>(typeId);
    }

    static public new BudgetAccountType Parse(string typeName) {
      return BudgetAccountType.Parse<BudgetAccountType>(typeName);
    }

    #endregion Constructors and parsers

  } // class BudgetAccountType

} // namespace Empiria.Budgeting
