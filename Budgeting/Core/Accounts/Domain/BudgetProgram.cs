/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Common Storage Type                     *
*  Type     : BudgetProgram                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a budget program.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting {

  /// <summary>Represents a budget program.</summary>
  public class BudgetProgram : CommonStorage {

    #region Constructors and parsers

    static public BudgetProgram Parse(int id) => ParseId<BudgetProgram>(id);

    static public BudgetProgram Parse(string uid) => ParseKey<BudgetProgram>(uid);

    static public FixedList<BudgetProgram> GetList() {
      return GetStorageObjects<BudgetProgram>();
    }

    static public BudgetProgram ParseWithCode(string programCode) => ParseWithCode<BudgetProgram>(programCode);

    static public BudgetProgram Empty => ParseEmpty<BudgetProgram>();

    static public BudgetProgram Undefined => ParseWithCode<BudgetProgram>("N/D");

    #endregion Constructors and parsers

    #region Properties

    public new string Code {
      get {
        return base.Code;
      }
    }

    #endregion Properties

  }  // class BudgetProgram

}  // namespace Empiria.Budgeting
