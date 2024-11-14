/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Use cases Layer                         *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Use case interactor class               *
*  Type     : FixedAssetTransactionUseCases              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for fixed assets transactions.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

namespace Empiria.Assets.UseCases {

  /// <summary>Use cases for fixed assets transactions.</summary>
  public class FixedAssetTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected FixedAssetTransactionUseCases() {
      // no-op
    }

    static public FixedAssetTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FixedAssetTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<NamedEntityDto> GetFixedAssetTransactionTypes() {
      var transactionTypes = FixedAssetTransactionType.GetList();

      return transactionTypes.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class FixedAssetTransactionUseCases

}  // namespace Empiria.Assets.UseCases
