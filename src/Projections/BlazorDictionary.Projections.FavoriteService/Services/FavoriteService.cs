﻿using BlazorDictionary.Common.Events.Entry;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BlazorDictionary.Projections.FavoriteService.Services
{
    public class FavoriteService
    {
        private readonly string connectionString;

        public FavoriteService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task CreateEntryFav(CreateEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection
                .ExecuteAsync("INSERT INTO EntryFavorite (Id, EntryId, CreatedById, CreateDate) VALUES(@Id, @EntryId, @CreatedById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy
                });
        }
    }
}