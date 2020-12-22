using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGenerator.DataBaseNotes {
    static class DbNoteHelper {

        public static void AlterNote(string title, string tag, int priority, string text, int? id) {
            if (id != null) {
                using (DBContext db = new DBContext()) {
                    var note = db.Notes.Single(n => n.Note_id == id);
                    note.Note_text = text;
                    note.Note_tag = tag;
                    note.Note_priority = priority;
                    note.Note_title = title;
                    note.Note_edit_date = DateTime.Now;
                    db.SaveChanges();
                }
            }
            else return;

        }

        public static void DeleteNote(int? id) {
            if (id != null) {
                using (DBContext db = new DBContext()) {
                    var note = db.Notes.Single(x => x.Note_id == id);
                    db.Notes.Remove(note);
                    db.SaveChanges();
                }
            }
            else return;
        }

        public static List<Note> getSortNotesDate(bool isAscending) {
            using (DBContext db = new DBContext()) {
                var notes = db.Notes;
                List<Note> sortedNotes;
                if (isAscending) {
                    sortedNotes = notes.OrderBy(note => note.Note_date).ToList();
                }
                else {
                    sortedNotes = notes.OrderByDescending(note => note.Note_date).ToList();
                }
                return sortedNotes;
            }
        }

        public static List<Note> getSortNotesTitle(bool isAscending) {
            using (DBContext db = new DBContext()) {
                var notes = db.Notes;
                List<Note> sortedNotes;
                if (isAscending) {
                    sortedNotes = notes.OrderBy(note => note.Note_title).ToList();
                }
                else {
                    sortedNotes = notes.OrderByDescending(note => note.Note_title).ToList();
                }
                return sortedNotes;
            }
        }

        public static List<Note> getSortNotesPriority(bool isAscending) {
            using (DBContext db = new DBContext()) {
                var notes = db.Notes;
                List<Note> sortedNotes;
                if (isAscending) {
                    sortedNotes = notes.OrderBy(note => note.Note_priority).ToList();
                }
                else {
                    sortedNotes = notes.OrderByDescending(note => note.Note_priority).ToList();
                }
                return sortedNotes;
            }
        }
    }
}
