using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using PuppyYogaMaui2.Models;

namespace PuppyYogaMaui2.Data
{
    public class PuppyYogaDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public PuppyYogaDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<YogaClass>().Wait();
            _database.CreateTableAsync<Instructor>().Wait();
            _database.CreateTableAsync<Reservation>().Wait();
            _database.CreateTableAsync<ClassFeedback>().Wait();
            _database.CreateTableAsync<InstructorFeedback>().Wait();
        }

        public Task<List<YogaClass>> GetClassesAsync()
        {
            return _database.Table<YogaClass>().ToListAsync();
        }

        public async Task<int> SaveClassAsync(YogaClass yogaClass)
        {
            Console.WriteLine($"Saving class with ID: {yogaClass.Id}");
            if (yogaClass.Id != 0)
            {
                int rowsAffected = await _database.UpdateAsync(yogaClass);
                if (rowsAffected == 0) Console.WriteLine("No rows were updated.");
                return rowsAffected;
            }
            else
            {
                return await _database.InsertAsync(yogaClass);
            }
        }

        public Task<YogaClass> GetClassAsync(int id)
        {
            return _database.Table<YogaClass>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> DeleteClassAsync(YogaClass yogaClass)
        {
            return _database.DeleteAsync(yogaClass);
        }

        public Task<List<Instructor>> GetInstructorsAsync()
        {
            return _database.Table<Instructor>().ToListAsync();
        }

        public Task<Instructor> GetInstructorAsync(int id)
        {
            return _database.Table<Instructor>().Where(i => i.InstructorId == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveInstructorAsync(Instructor instructor)
        {
            if (instructor.InstructorId != 0)
            {
                return await _database.UpdateAsync(instructor);
            }
            else
            {
                return await _database.InsertAsync(instructor);
            }
        }

        public Task<int> DeleteInstructorAsync(Instructor instructor)
        {
            return _database.DeleteAsync(instructor);
        }
        public async Task<List<Reservation>> GetReservationsAsync()
        {
            var reservationsWithClasses = new List<Reservation>();
            var reservations = await _database.Table<Reservation>().ToListAsync();
            foreach (var reservation in reservations)
            {
                var yogaClass = await _database.Table<YogaClass>().Where(c => c.Id == reservation.YogaClassId).FirstOrDefaultAsync();
                if (yogaClass != null)
                {
                    reservation.ClassName = yogaClass.Name;
                    reservation.ClassDate = yogaClass.ScheduleDate;
                    reservation.ClassTime = yogaClass.ScheduleTime;
                }
                reservationsWithClasses.Add(reservation);
            }
            return reservationsWithClasses;
        }

        public async Task<bool> SaveReservationAsync(Reservation reservation)
        {
            int result = reservation.Id != 0
                ? await _database.UpdateAsync(reservation)
                : await _database.InsertAsync(reservation);
            return result > 0;
        }
        public Task<int> DeleteReservationAsync(Reservation reservation)
        {
            return _database.DeleteAsync(reservation);
        }
        public async Task SaveInstructorFeedbackAsync(InstructorFeedback feedback)
        {
            await _database.InsertAsync(feedback);
        }
        public Task<List<InstructorFeedback>> GetInstructorFeedbackAsync(int instructorId)
        {
            return _database.Table<InstructorFeedback>()
                            .Where(f => f.InstructorId == instructorId)
                            .ToListAsync();
        }
        public async Task UpdateInstructorFeedbackAsync(InstructorFeedback feedback)
        {
            await _database.UpdateAsync(feedback);
        }
        public async Task SaveClassFeedbackAsync(ClassFeedback feedback)
        {
            await _database.InsertAsync(feedback);
        }
        public Task<List<ClassFeedback>> GetClassFeedbackAsync(int classId)
        {
            return _database.Table<ClassFeedback>()
                .Where(f => f.YogaClassId == classId).ToListAsync();
        }
        public async Task UpdateClassFeedbackAsync(ClassFeedback feedback)
        {
            await _database.UpdateAsync(feedback);
        }
    }
}
