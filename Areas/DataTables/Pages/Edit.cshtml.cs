﻿using AutoMapper;
using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class EditModel : PageModel
    {
        private readonly mcs_homesiteContext _context;
        private readonly IMapper _mapper;

        public EditModel(mcs_homesiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public User UserModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDto =  await _context.UserDto.FirstOrDefaultAsync(m => m.Id == id);
            if (userDto == null)
            {
                return NotFound();
            }
            UserModel = _mapper.Map<User>(userDto);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(_mapper.Map<UserDto>(UserModel)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDtoExists(UserModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserDtoExists(long? id)
        {
          return (id != null) && (_context.UserDto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
