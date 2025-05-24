/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : CreditAccountUseCases                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve credit accounts.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Adapters;

namespace Empiria.Financial.UseCases {

  /// <summary>Provides use cases for update and retrieve credit accounts.</summary>
  public class CreditAccountUseCases : UseCase {

    #region Constructors and parsers

    protected CreditAccountUseCases() {
      // no-op
    }

    static public CreditAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CreditAccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FinancialAccountDto UpdateCreditData(string creditAccountUID, CreditExtDataFields fields) {
      Assertion.Require(creditAccountUID, nameof(creditAccountUID));
      Assertion.Require(fields, nameof(fields));

      var credit = CreditAccount.Parse(creditAccountUID);

      credit.Update(fields);

      credit.Save();

      return FinancialAccountMapper.Map(credit);
    }

    #endregion Use cases

  }  // class FinancialAccountUseCases

}  // namespace Empiria.Financial.UseCases
