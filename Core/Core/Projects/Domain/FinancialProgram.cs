/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned type                        *
*  Type     : FinancialProgram                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial program.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Projects {

  /// <summary>Represents a financial program.</summary>
  public class FinancialProgram : INamedEntity {

    static private readonly FixedList<StandardAccount> _allSubprograms;

    #region Constructors and parsers

    static FinancialProgram() {
      var classificator = FinancialProjectClassificator.Subprograma;

      var category = CommonStorage.ParseNamedKey<StandardAccountCategory>(classificator.ToString());

      _allSubprograms = category.GetStandardAccounts();
    }


    public FinancialProgram(StandardAccount standardAccount) {
      UID = standardAccount.UID;
      Name = standardAccount.Name;
    }

    private FinancialProgram(StandardAccount standardAccount, string categoryCode) {
      UID = standardAccount.UID;
      Name = standardAccount.Name;
      Subprograms = standardAccount.GetChildren()
                                   .FindAll(x => x.StdAcctNo.Substring(3) == categoryCode)
                                   .Select(x => new FinancialProgram(x)).ToFixedList();
    }



    static public FixedList<FinancialProgram> GetList(FinancialProjectCategory category) {
      return _allSubprograms.FindAll(x => x.StdAcctNo.Substring(3) == category.StandardAccountCode)
                            .SelectDistinct(x => x.Parent)
                            .Select(x => new FinancialProgram(x, category.StandardAccountCode))
                            .ToFixedList();
    }

    #endregion Constructors and parsers

    public string UID {
      get;
    }

    public string Name {
      get;
    }

    public FixedList<FinancialProgram> Subprograms {
      get;
    } = new FixedList<FinancialProgram>();

  } // class FinancialProgram

} // namespace Empiria.Financial.Projects
