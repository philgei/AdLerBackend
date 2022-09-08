﻿using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.Repositories.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly BaseAdLerBackendDbContext Context;

    public GenericRepository(BaseAdLerBackendDbContext dbContext)
    {
        Context = dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<T?> GetAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }
}