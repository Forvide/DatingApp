using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await _context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(p => p.Id == id);
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotosAsync()
        {
            return await _context.Photos.IgnoreQueryFilters().Where(p => p.IsApproved == false)
                .Select(u => new PhotoForApprovalDto{
                    Id = u.Id,
                    Url = u.Url,
                    Username = u.AppUser.UserName,
                    IsApproved = u.IsApproved
                }).ToListAsync();
        }
    }
}