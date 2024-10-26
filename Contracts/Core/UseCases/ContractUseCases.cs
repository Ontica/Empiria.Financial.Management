/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Use cases Layer                         *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : ContractUseCases                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for contract management.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Contracts.Data;
using Empiria.Contracts.Adapters;

namespace Empiria.Contracts.UseCases {

  /// <summary>Use cases for contract management.</summary>
  public class ContractUseCases : UseCase {

    #region Constructors and parsers

    protected ContractUseCases() {
      // no-op
    }

    static public ContractUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ContractUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public ContractDto AddContract(ContractFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var contract = new Contract();

      contract.Load(fields);

      contract.SetDates(fields.SignDate, contract.FromDate, fields.ToDate);

      contract.Save();

      return ContractMapper.Map(contract);
    }

    public ContractDto GetContract(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = Contract.Parse(contractUID);

      return ContractMapper.Map(contract);
    }


    public FixedList<ContractItemDto> GetContractItems(string contractUID) {
      Assertion.Require(contractUID, nameof(contractUID));

      var contract = Contract.Parse(contractUID);

      FixedList<ContractItem> items = contract.GetItems();

      return ContractItemMapper.Map(items);
    }


    public FixedList<NamedEntityDto> GetContractTypes() {
      var contractTypes = ContractType.GetList();

      return contractTypes.MapToNamedEntityList();
    }


    public FixedList<ContractDescriptor> SearchContracts(ContractQuery query) {
      Assertion.Require(query, nameof(query));

      string condition = query.MapToFilterString();
      string orderBy = query.MapToSortString();

      FixedList<Contract> contracts = ContractData.getContracts(condition, orderBy);

      return ContractMapper.MapToDescriptor(contracts);
    }

    #endregion Use cases

  }  // class ContractUseCases

}  // namespace Empiria.Contracts.UseCases
