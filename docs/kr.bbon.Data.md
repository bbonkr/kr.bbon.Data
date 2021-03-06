# kr.bbon.Data Package


[![](https://img.shields.io/nuget/v/kr.bbon.Data)](https://www.nuget.org/packages/kr.bbon.Data) [![](https://img.shields.io/nuget/dt/kr.bbon.Data)](https://www.nuget.org/packages/kr.bbon.Data) [![publish to nuget](https://github.com/bbonkr/kr.bbon.Data/actions/workflows/build-tag.yaml/badge.svg)](https://github.com/bbonkr/kr.bbon.Data/actions/workflows/build-tag.yaml)

## π’ Overview

## π Namespace

### kr.bbon.Data

DbContext κΈ°λ°μ΄ λλ AppDbContextBase ν΄λμ€μ IEntityTypeConfiguration μΈν°νμ΄μ€ μΌλΆκ° κ΅¬νλ EntityTypeConfigurationBase ν΄λμ€λ₯Ό μ κ³΅ν©λλ€.

AppDbContextBase λ Soft deletion μ μ§μνλ μν°ν°μ λν μ²λ¦¬ λ±μ΄ ν¬ν¨λμ΄ μμ΅λλ€.

μμ±νλ €λ DbContext ν΄λμ€λ AppDbContextBase ν΄λμ€ κΈ°λ°μΌλ‘ νμν κΈ°λ₯μ νμ₯ν©λλ€.

EntityTypeConfigurationBase λ kr.bbon.Data.Abstractions.Entities λ€μμ€νμ΄μ€μμ μ κ³΅λλ νμμΌλ‘ κ΅¬ν λλ νμ₯λ μν°ν°μ λν IEntityTypeConfiguration μΈν°νμ΄μ€μ μΌλΆ κ΅¬νμ΄ μ κ³΅λ©λλ€.

### kr.bbon.Data.Repositories

kr.bbon.Data.Abstractions.IRepository μΈν°νμ΄μ€λ₯Ό κ΅¬ννλ λ ν¬μ§ν λ¦¬ ν΄λμ€κ° μ κ³΅λ©λλ€.

λ°μ΄ν° μΏΌλ¦¬λ₯Ό μν κΈ°λ₯μ Specification ν¨ν΄μ μ¬μ©νκ² μ€λΉλμ΄ μμ΅λλ€.

### kr.bbon.Data.Services

Unit of work κ΅¬νμ μ μ ν΄μμ λν DataServiceBase ν΄λμ€κ° μ κ³΅λ©λλ€.

IRepository μΈν°νμ΄μ€μ κ΅¬ν νμμ νλλ‘ μΆκ°ν΄μ μ¬μ©ν©λλ€.

### kr.bbon.Data.Extensions.DependencyInjection

μμ‘΄μ± κ΅¬μ±μ μν IServiceCollection νμ₯κ³Ό Specification ν¨ν΄ κ΅¬νμ μν IQueryable νμ₯μ΄ μ κ³΅λ©λλ€.

## π― Features

### AppDbContextBase 

DbContext κΈ°λ° ν΄λμ€μλλ€.

DbSet<TEntity> νλλ₯Ό μΆκ°νκ³ , OnModelCreating λ©μλλ₯Ό μ¬μ μν΄μ EntityTypeConfiguration μ΄ κ΅¬νλ μ΄μλΈλ¦¬λ₯Ό νμ©νκ² κ³νλμ΄ μμ΅λλ€.

### EntityTypeConfigurationBase 

kr.bbon.Data.Abstractions.Entities λ€μμ€νμ΄μ€μ μ€λΉλ μν°ν° κΈ°λ° νμμΌλ‘ μν°ν° ν΄λμ€λ₯Ό μ μνλ©΄, λ―Έλ¦¬ μ μλ κΈ°λ₯μ λν μν°ν° νμ κ΅¬μ±μ μ€λΉλ κ΅¬μ±μ μ κ³΅ν©λλ€.

### RepositoryBase

λ ν¬μ§ν λ¦¬ ν¨ν΄μ κ΅¬ννλ κΈ°λ° ν΄λμ€λ₯Ό μ κ³΅ν©λλ€.

λ°μ΄ν° μΆλ ₯μ Specification ν¨ν΄μΌλ‘ μΏΌλ¦¬λ₯Ό μ μν  μ μκ² SpecificationBase ν΄λμ€λ₯Ό μ¬μ©ν©λλ€.

λ°μ΄ν° μλ ₯μ EntityFrameworkCoreμ κΈ°λ³Έ κΈ°λ₯μ μ¬μ©νλ©°, νΉμ΄μ¬ν­μ μμ΅λλ€.

### DataServiceBase 

Unit of work κ΅¬νμ λν μ μ ν΄μμΌλ‘ κΈ°λ° ν΄λμ€λ₯Ό μ κ³΅ν©λλ€.

λ°μ΄ν° μ²λ¦¬μ νμν Repository λ μμ±μ μ£ΌμμΌλ‘ κ΅¬μ±νκ³ , κ° Repository λ νλλ‘ μ μν΄μ μΈλΆμμ μ κ·Όν  μ μκ² κ΅¬ννκ² μ€λΉλμ΄ μμ΅λλ€.

DataService μ DbContext κ° μμ΄, μ¬λ¬ μ μ₯μμ νΈλμ­μμ νλλ‘ μ²λ¦¬ν  μ μμ΅λλ€.

DataServie κ΅¬ν νλ‘μ νΈμ λλ©μΈ κΈ°λ₯ νλ‘μ νΈλ₯Ό λΆλ¦¬ν  μ μκ² κ΅¬μ±νλ κ²μ μΆμ²ν©λλ€.

## μ°Έμ‘°

μλ£¨μμ μ΄κ³ , Example1 μλ£¨μ λλ ν°λ¦¬μ νλ‘μ νΈλ₯Ό νμΈνμ­μμ€.