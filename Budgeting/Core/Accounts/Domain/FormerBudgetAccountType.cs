/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power type                              *
*  Type     : BudgetAccountType                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget account.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Budgeting {

  /// <summary>Power type that describes a budget account.</summary>
  [Powertype(typeof(FormerBudgetAccount))]
  public sealed class FormerBudgetAccountType : Powertype {

    #region Constructors and parsers

    private FormerBudgetAccountType() {
      // Empiria powertype types always have this constructor.
    }

    static public new FormerBudgetAccountType Parse(int typeId) => Parse<FormerBudgetAccountType>(typeId);

    static public new FormerBudgetAccountType Parse(string typeName) => Parse<FormerBudgetAccountType>(typeName);

    public static FormerBudgetAccountType GastoCorriente => Parse("ObjectTypeInfo.FormerBudgetAccount.GastoCorriente");

    #endregion Constructors and parsers

  } // class BudgetAccountType

} // namespace Empiria.Budgeting
