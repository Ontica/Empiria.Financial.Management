/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : ContractItemUseCases                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract items management.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Contracts.Adapters;

namespace Empiria.Contracts.UseCases {

  /// <summary>Use cases for contract items management.</summary>
  public class ContractItemUseCases : UseCase {

    #region Constructors and parsers

    protected ContractItemUseCases() {
      // no-op
    }

    static public ContractItemUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ContractItemUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractItemDto AddContractItem(string ContractUID, ContractItemFields fields) {

      Assertion.Require(ContractUID, nameof(ContractUID));
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = Contract.Parse(ContractUID);

      var contractItem = new ContractItem(fields);

      contract.AddItem(contractItem);

      contractItem.Save();

      return ContractItemMapper.Map(contractItem);
    }


    public void DeleteContractItem(string ContractItemUID) {

      Assertion.Require(ContractItemUID, nameof(ContractItemUID));

      var contractItem = ContractItem.Parse(ContractItemUID);

      contractItem.Delete();

      contractItem.Save();
    }


    public ContractItemDto GetContractItem(string contractItemUID) {

      Assertion.Require(contractItemUID, nameof(contractItemUID));

      var contractItem = ContractItem.Parse(contractItemUID);

      return ContractItemMapper.Map(contractItem);
    }


    public ContractItemDto UpdateContractItem(string ContractItemUID,
                                              ContractItemFields fields) {

      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contractItem = ContractItem.Parse(ContractItemUID);

      contractItem.Load(fields);

      contractItem.Save();

      return ContractItemMapper.Map(contractItem);
    }

    #endregion Use cases

  }  // class ContractUseCases

}  // namespace Empiria.Contracts.UseCases
