using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGC_OnBoarding.Models.ApiModels
{
    public class CompanyDetailModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Activity
        {
            public string code { get; set; }
            public string description { get; set; }
        }

        public class Activity2
        {
            public string code { get; set; }
            public string industrySector { get; set; }
            public string description { get; set; }
            public string classification { get; set; }
        }

        public class ActivityClassification
        {
            public string classification { get; set; }
            public List<Activity> activities { get; set; }
        }

        public class AdditionalData
        {
        }

        public class AdditionalInformation
        {
        }

        public class Address
        {
            public string type { get; set; }
            public string simpleValue { get; set; }
            public string street { get; set; }
            public string houseNo { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
            public string province { get; set; }
            public string telephone { get; set; }
            public bool directMarketingOptOut { get; set; }
            public bool directMarketingOptIn { get; set; }
            public string country { get; set; }
        }

        public class Advisor
        {
            public string auditorName { get; set; }
            public string solicitorName { get; set; }
            public string accountantName { get; set; }
        }

        public class AffiliatedCompany
        {
            public string country { get; set; }
            public string id { get; set; }
            public string safeNo { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string officeType { get; set; }
            public string status { get; set; }
            public string regNo { get; set; }
            public string vatNo { get; set; }
            public Address address { get; set; }
            public Activity activity { get; set; }
            public string legalForm { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class AlternateSummary
        {
        }

        public class BalanceSheet
        {
            public int landAndBuildings { get; set; }
            public int plantAndMachinery { get; set; }
            public int otherTangibleAssets { get; set; }
            public int totalTangibleAssets { get; set; }
            public int goodwill { get; set; }
            public int otherIntangibleAssets { get; set; }
            public int totalIntangibleAssets { get; set; }
            public int investments { get; set; }
            public int loansToGroup { get; set; }
            public int otherLoans { get; set; }
            public int miscellaneousFixedAssets { get; set; }
            public int totalOtherFixedAssets { get; set; }
            public int totalFixedAssets { get; set; }
            public int rawMaterials { get; set; }
            public int workInProgress { get; set; }
            public int finishedGoods { get; set; }
            public int otherInventories { get; set; }
            public int totalInventories { get; set; }
            public int tradeReceivables { get; set; }
            public int groupReceivables { get; set; }
            public int receivablesDueAfter1Year { get; set; }
            public int miscellaneousReceivables { get; set; }
            public int totalReceivables { get; set; }
            public int cash { get; set; }
            public int otherCurrentAssets { get; set; }
            public int totalCurrentAssets { get; set; }
            public int totalAssets { get; set; }
            public int tradePayables { get; set; }
            public int bankLiabilities { get; set; }
            public int otherLoansOrFinance { get; set; }
            public int groupPayables { get; set; }
            public int miscellaneousLiabilities { get; set; }
            public int totalCurrentLiabilities { get; set; }
            public int tradePayablesDueAfter1Year { get; set; }
            public int bankLiabilitiesDueAfter1Year { get; set; }
            public int otherLoansOrFinanceDueAfter1Year { get; set; }
            public int groupPayablesDueAfter1Year { get; set; }
            public int miscellaneousLiabilitiesDueAfter1Year { get; set; }
            public int totalLongTermLiabilities { get; set; }
            public int totalLiabilities { get; set; }
            public int calledUpShareCapital { get; set; }
            public int sharePremium { get; set; }
            public int revenueReserves { get; set; }
            public int otherReserves { get; set; }
            public int totalShareholdersEquity { get; set; }
        }

        public class Banker
        {
            public string name { get; set; }
            public Address address { get; set; }
            public string bankCode { get; set; }
            public string bic { get; set; }
        }

        public class BasicInformation
        {
            public string businessName { get; set; }
            public string registeredCompanyName { get; set; }
            public string companyRegistrationNumber { get; set; }
            public string country { get; set; }
            public string vatRegistrationNumber { get; set; }
            public DateTime vatRegistrationDate { get; set; }
            public DateTime companyRegistrationDate { get; set; }
            public DateTime operationsStartDate { get; set; }
            public string commercialCourt { get; set; }
            public LegalForm legalForm { get; set; }
            public string ownershipType { get; set; }
            public CompanyStatus companyStatus { get; set; }
            public PrincipalActivity principalActivity { get; set; }
            public ContactAddress contactAddress { get; set; }
        }

        public class CompanyIdentification
        {
            public BasicInformation basicInformation { get; set; }
            public List<ActivityClassification> activityClassifications { get; set; }
            public List<PreviousName> previousNames { get; set; }
            public List<PreviousLegalForm> previousLegalForms { get; set; }
        }

        public class CompanyStatus
        {
            public string status { get; set; }
            public string description { get; set; }
        }

        public class CompanySummary
        {
            public string businessName { get; set; }
            public string country { get; set; }
            public string companyNumber { get; set; }
            public string companyRegistrationNumber { get; set; }
            public MainActivity mainActivity { get; set; }
            public CompanyStatus companyStatus { get; set; }
            public LatestTurnoverFigure latestTurnoverFigure { get; set; }
            public LatestShareholdersEquityFigure latestShareholdersEquityFigure { get; set; }
            public CreditRating creditRating { get; set; }
        }

        public class ContactAddress
        {
            public string type { get; set; }
            public string simpleValue { get; set; }
            public string street { get; set; }
            public string houseNo { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
            public string province { get; set; }
            public string telephone { get; set; }
            public bool directMarketingOptOut { get; set; }
            public bool directMarketingOptIn { get; set; }
            public string country { get; set; }
        }

        public class ContactInformation
        {
            public MainAddress mainAddress { get; set; }
            public List<OtherAddress> otherAddresses { get; set; }
            public List<PreviousAddress> previousAddresses { get; set; }
            public List<string> emailAddresses { get; set; }
            public List<string> websites { get; set; }
        }

        public class CreditLimit
        {
            public string currency { get; set; }
            public string value { get; set; }
        }

        public class CreditRating
        {
            public string commonValue { get; set; }
            public string commonDescription { get; set; }
            public CreditLimit creditLimit { get; set; }
            public ProviderValue providerValue { get; set; }
            public string providerDescription { get; set; }
            public int pod { get; set; }
            public string assessment { get; set; }
        }

        public class CreditScore
        {
            public CurrentCreditRating currentCreditRating { get; set; }
            public CurrentContractLimit currentContractLimit { get; set; }
            public PreviousCreditRating previousCreditRating { get; set; }
            public DateTime latestRatingChangeDate { get; set; }
        }

        public class CurrentContractLimit
        {
            public string currency { get; set; }
            public int value { get; set; }
        }

        public class CurrentCreditRating
        {
            public string commonValue { get; set; }
            public string commonDescription { get; set; }
            public CreditLimit creditLimit { get; set; }
            public ProviderValue providerValue { get; set; }
            public string providerDescription { get; set; }
            public string pod { get; set; }
            public string assessment { get; set; }
        }

        public class CurrentDirector
        {
            public string id { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string firstNames { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string surname { get; set; }
            public Address address { get; set; }
            public string gender { get; set; }
            public string birthName { get; set; }
            public DateTime dateOfBirth { get; set; }
            public string placeOfBirth { get; set; }
            public string nationality { get; set; }
            public string countryOfResidence { get; set; }
            public string country { get; set; }
            public string directorType { get; set; }
            public bool hasNegativeInfo { get; set; }
            public bool signingAuthority { get; set; }
            public List<Position> positions { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class CurrentDirectorship
        {
            public string id { get; set; }
            public string title { get; set; }
            public string initials { get; set; }
            public string name { get; set; }
            public Position position { get; set; }
            public string registrationNumber { get; set; }
            public string companyName { get; set; }
            public Status status { get; set; }
        }

        public class Directors
        {
            public List<CurrentDirector> currentDirectors { get; set; }
            public List<PreviousDirector> previousDirectors { get; set; }
        }

        public class DirectorsExtra
        {
        }

        public class Directorships
        {
            public List<CurrentDirectorship> currentDirectorships { get; set; }
            public List<PreviousDirectorship> previousDirectorships { get; set; }
        }

        public class EmployeesInformation
        {
            public int year { get; set; }
            public string numberOfEmployees { get; set; }
        }

        public class ExtendedGroupStructure
        {
            public string id { get; set; }
            public string country { get; set; }
            public string safeNumber { get; set; }
            public string idType { get; set; }
            public string companyName { get; set; }
            public string registeredNumber { get; set; }
            public DateTime latestAnnualAccounts { get; set; }
            public int level { get; set; }
            public int percentOfOwnership { get; set; }
            public string status { get; set; }
            public string commonRatingBand { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class ExtendedGroupStructureExtra
        {
        }

        public class FinancialStatement
        {
            public DateTime yearEndDate { get; set; }
            public int numberOfWeeks { get; set; }
            public string currency { get; set; }
            public bool consolidatedAccounts { get; set; }
            public ProfitAndLoss profitAndLoss { get; set; }
            public BalanceSheet balanceSheet { get; set; }
            public OtherFinancials otherFinancials { get; set; }
            public Ratios ratios { get; set; }
        }

        public class GroupStructure
        {
            public UltimateParent ultimateParent { get; set; }
            public ImmediateParent immediateParent { get; set; }
            public List<SubsidiaryCompany> subsidiaryCompanies { get; set; }
            public List<AffiliatedCompany> affiliatedCompanies { get; set; }
        }

        public class ImmediateParent
        {
            public string country { get; set; }
            public string id { get; set; }
            public string safeNo { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string officeType { get; set; }
            public string status { get; set; }
            public string regNo { get; set; }
            public string vatNo { get; set; }
            public Address address { get; set; }
            public Activity activity { get; set; }
            public string legalForm { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class IssuedShareCapital
        {
            public string currency { get; set; }
            public int value { get; set; }
        }

        public class LatestShareholdersEquityFigure
        {
            public string currency { get; set; }
            public int value { get; set; }
        }

        public class LatestTurnoverFigure
        {
            public string currency { get; set; }
            public int value { get; set; }
        }

        public class LegalForm
        {
            public string commonCode { get; set; }
            public string providerCode { get; set; }
            public string description { get; set; }
        }

        public class LocalFinancialStatement
        {
            public DateTime yearEndDate { get; set; }
            public int numberOfWeeks { get; set; }
            public string currency { get; set; }
            public bool consolidatedAccounts { get; set; }
        }

        public class MainActivity
        {
            public string code { get; set; }
            public string industrySector { get; set; }
            public string description { get; set; }
            public string classification { get; set; }
        }

        public class MainAddress
        {
            public string type { get; set; }
            public string simpleValue { get; set; }
            public string street { get; set; }
            public string houseNo { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
            public string province { get; set; }
            public string telephone { get; set; }
            public bool directMarketingOptOut { get; set; }
            public bool directMarketingOptIn { get; set; }
            public string country { get; set; }
        }

        public class Message
        {
            public string type { get; set; }
            public string code { get; set; }
            public string text { get; set; }
        }

        public class NegativeInformation
        {
        }

        public class NegativeInformationExtra
        {
        }

        public class NominalShareCapital
        {
            public string currency { get; set; }
            public int value { get; set; }
        }

        public class OtherAddress
        {
            public string type { get; set; }
            public string simpleValue { get; set; }
            public string street { get; set; }
            public string houseNo { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
            public string province { get; set; }
            public string telephone { get; set; }
            public bool directMarketingOptOut { get; set; }
            public bool directMarketingOptIn { get; set; }
            public string country { get; set; }
        }

        public class OtherFinancials
        {
            public string contingentLiabilities { get; set; }
            public int workingCapital { get; set; }
            public int netWorth { get; set; }
        }

        public class OtherInformation
        {
            public List<Banker> bankers { get; set; }
            public List<Advisor> advisors { get; set; }
            public List<EmployeesInformation> employeesInformation { get; set; }
        }

        public class PaymentData
        {
        }

        public class PaymentDataExtra
        {
        }

        public class Position
        {
            public DateTime dateAppointed { get; set; }
            public string commonCode { get; set; }
            public string providerCode { get; set; }
            public string positionName { get; set; }
            public string authority { get; set; }
            public string apptDurationType { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class Position3
        {
            public DateTime dateAppointed { get; set; }
            public string commonCode { get; set; }
            public string providerCode { get; set; }
            public string positionName { get; set; }
            public string authority { get; set; }
            public string apptDurationType { get; set; }
            public AdditionalData additionalData { get; set; }
            public DateTime resignationDate { get; set; }
        }

        public class PreviousAddress
        {
            public string type { get; set; }
            public string simpleValue { get; set; }
            public string street { get; set; }
            public string houseNo { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
            public string province { get; set; }
            public string telephone { get; set; }
            public bool directMarketingOptOut { get; set; }
            public bool directMarketingOptIn { get; set; }
            public string country { get; set; }
        }

        public class PreviousCreditRating
        {
            public string commonValue { get; set; }
            public string commonDescription { get; set; }
            public CreditLimit creditLimit { get; set; }
            public ProviderValue providerValue { get; set; }
            public string providerDescription { get; set; }
            public int pod { get; set; }
            public string assessment { get; set; }
        }

        public class PreviousDirector
        {
            public string id { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string firstNames { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string surname { get; set; }
            public Address address { get; set; }
            public string gender { get; set; }
            public string birthName { get; set; }
            public DateTime dateOfBirth { get; set; }
            public string placeOfBirth { get; set; }
            public string nationality { get; set; }
            public string countryOfResidence { get; set; }
            public string country { get; set; }
            public string directorType { get; set; }
            public bool hasNegativeInfo { get; set; }
            public bool signingAuthority { get; set; }
            public List<Position> positions { get; set; }
            public AdditionalData additionalData { get; set; }
            public DateTime resignationDate { get; set; }
        }

        public class PreviousDirectorship
        {
            public string id { get; set; }
            public string title { get; set; }
            public string initials { get; set; }
            public string name { get; set; }
            public Position position { get; set; }
            public string registrationNumber { get; set; }
            public string companyName { get; set; }
            public Status status { get; set; }
        }

        public class PreviousLegalForm
        {
            public DateTime dateChanged { get; set; }
            public LegalForm legalForm { get; set; }
        }

        public class PreviousName
        {
            public DateTime dateChanged { get; set; }
            public string name { get; set; }
        }

        public class PrincipalActivity
        {
            public string code { get; set; }
            public string industrySector { get; set; }
            public string description { get; set; }
            public string classification { get; set; }
        }

        public class ProfitAndLoss
        {
            public int revenue { get; set; }
            public int operatingCosts { get; set; }
            public int operatingProfit { get; set; }
            public int wagesAndSalaries { get; set; }
            public int pensionCosts { get; set; }
            public int depreciation { get; set; }
            public int amortisation { get; set; }
            public int financialIncome { get; set; }
            public int financialExpenses { get; set; }
            public int extraordinaryIncome { get; set; }
            public int extraordinaryCosts { get; set; }
            public int profitBeforeTax { get; set; }
            public int tax { get; set; }
            public int profitAfterTax { get; set; }
            public int dividends { get; set; }
            public int minorityInterests { get; set; }
            public int otherAppropriations { get; set; }
            public int retainedProfit { get; set; }
        }

        public class ProviderValue
        {
            public string maxValue { get; set; }
            public string minValue { get; set; }
            public string value { get; set; }
        }

        public class Ratios
        {
            public int preTaxProfitMargin { get; set; }
            public int returnOnCapitalEmployed { get; set; }
            public int returnOnTotalAssetsEmployed { get; set; }
            public int returnOnNetAssetsEmployed { get; set; }
            public int salesOrNetWorkingCapital { get; set; }
            public decimal stockTurnoverRatio { get; set; }
            public int debtorDays { get; set; }
            public int creditorDays { get; set; }
            public decimal currentRatio { get; set; }
            public decimal liquidityRatioOrAcidTest { get; set; }
            public decimal currentDebtRatio { get; set; }
            public int gearing { get; set; }
            public decimal equityInPercentage { get; set; }
            public decimal totalDebtRatio { get; set; }
        }

        public class Report
        {
            public string companyId { get; set; }
            public string language { get; set; }
            public CompanySummary companySummary { get; set; }
            public CompanyIdentification companyIdentification { get; set; }
            public CreditScore creditScore { get; set; }
            public ContactInformation contactInformation { get; set; }
            public ShareCapitalStructure shareCapitalStructure { get; set; }
            public Directors directors { get; set; }
            public Directorships directorships { get; set; }
            public OtherInformation otherInformation { get; set; }
            public GroupStructure groupStructure { get; set; }
            public List<ExtendedGroupStructure> extendedGroupStructure { get; set; }
            public List<FinancialStatement> financialStatements { get; set; }
            public List<LocalFinancialStatement> localFinancialStatements { get; set; }
            public NegativeInformation negativeInformation { get; set; }
            public AdditionalInformation additionalInformation { get; set; }
            public DirectorsExtra directorsExtra { get; set; }
            public ExtendedGroupStructureExtra extendedGroupStructureExtra { get; set; }
            public PaymentData paymentData { get; set; }
            public PaymentDataExtra paymentDataExtra { get; set; }
            public AlternateSummary alternateSummary { get; set; }
            public NegativeInformationExtra negativeInformationExtra { get; set; }
        }

        public class Root
        {
            public string chargeReference { get; set; }
            public List<Message> messages { get; set; }
            public List<string> failedSections { get; set; }
            public Report report { get; set; }
        }

        public class ShareCapitalStructure
        {
            public NominalShareCapital nominalShareCapital { get; set; }
            public IssuedShareCapital issuedShareCapital { get; set; }
            public string shareCapitalCurrency { get; set; }
            public int numberOfSharesIssued { get; set; }
            public List<ShareHolder> shareHolders { get; set; }
        }

        public class ShareClass
        {
            public string shareType { get; set; }
            public string currency { get; set; }
            public int valuePerShare { get; set; }
            public bool jointlyOwned { get; set; }
            public int numberOfSharesOwned { get; set; }
            public int valueOfSharesOwned { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class ShareHolder
        {
            public string id { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string firstNames { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string surname { get; set; }
            public Address address { get; set; }
            public string shareholderType { get; set; }
            public string shareType { get; set; }
            public string currency { get; set; }
            public int totalValueOfSharesOwned { get; set; }
            public int totalNumberOfSharesOwned { get; set; }
            public int percentSharesHeld { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public bool hasNegativeInfo { get; set; }
            public List<ShareClass> shareClasses { get; set; }
        }

        public class Status
        {
            public string status { get; set; }
            public string description { get; set; }
        }

        public class SubsidiaryCompany
        {
            public string country { get; set; }
            public string id { get; set; }
            public string safeNo { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string officeType { get; set; }
            public string status { get; set; }
            public string regNo { get; set; }
            public string vatNo { get; set; }
            public Address address { get; set; }
            public Activity activity { get; set; }
            public string legalForm { get; set; }
            public AdditionalData additionalData { get; set; }
        }

        public class UltimateParent
        {
            public string country { get; set; }
            public string id { get; set; }
            public string safeNo { get; set; }
            public string idType { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string officeType { get; set; }
            public string status { get; set; }
            public string regNo { get; set; }
            public string vatNo { get; set; }
            public Address address { get; set; }
            public Activity activity { get; set; }
            public string legalForm { get; set; }
            public AdditionalData additionalData { get; set; }
        }


    }
}
