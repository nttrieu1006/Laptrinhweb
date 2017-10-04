﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDocTruyenOnline.Common;
using WebDocTruyenOnline.Models;

namespace WebDocTruyenOnline.Areas.Admin.Controllers
{
    public class ChapStoriesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ChapStories
        public ActionResult Index(string searchString)
        {
           
            IQueryable<ChapStory> model = db.ChapStories;
             model = model.Include(s => s.Story);
            return View(model.OrderBy(x=>x.StoryId).ThenBy(x=>x.Chap).ToList());
        }

        // GET: Admin/ChapStories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChapStory chapStory = db.ChapStories.Find(id);
            if (chapStory == null)
            {
                return HttpNotFound();
            }
           
            return View(chapStory);
        }

        // GET: Admin/ChapStories/Create
        public ActionResult Create()
        {
            
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Name");
            return View();
        }

        // POST: Admin/ChapStories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,StoryId,Chap,ChapName, MetaTitle,Content,CreateDate,CreateBy,ModifyDate,ModifyBy,Status")] ChapStory chapStory)
        {
            if (ModelState.IsValid)
            {
                var sess = (UserLogin)Session[CommonConstants.USER_SESSION];
                chapStory.CreateDate = DateTime.Now;
                chapStory.CreateBy = sess.Email;
                chapStory.Status = true;

                db.ChapStories.Add(chapStory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoryID = new SelectList(db.Stories, "Id", "Name", chapStory.StoryId);
            return View(chapStory);
        }

        // GET: Admin/ChapStories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChapStory chapStory = db.ChapStories.Find(id);
            if (chapStory == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Name", chapStory.StoryId);
            return View(chapStory);
        }

        // POST: Admin/ChapStories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,StoryId,Chap,ChapName,MetaTitle,Content,CreateDate,CreateBy,ModifyDate,ModifyBy,Status")] ChapStory chapStory)
        {
            if (ModelState.IsValid)
            {
               
                var sess = (UserLogin)Session[CommonConstants.USER_SESSION];
                chapStory.ModifyDate = DateTime.Now;
                chapStory.ModifyBy = sess.Email;
                var st = db.Stories.Find(chapStory.StoryId);
                chapStory.MetaTitle = st.MetaTitle;

                db.Entry(chapStory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoryId = new SelectList(db.Stories, "Id", "Name", chapStory.StoryId);
            return View(chapStory);
        }

        ////GET: Admin/Stories/MultiDelete
        public ActionResult MultiDelete(string searchString)
        {
            IQueryable<ChapStory> model = db.ChapStories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MetaTitle.Contains(searchString));
            }
            
            model = model.Include(s => s.Story);
            return View(model.ToList());
        }
        // POST: Admin/Stories/MultiDelete
        [HttpPost]
        public ActionResult MultiDelete(int[] chkId)
        {
            for (int i = 0; i < chkId.Length; i++)
            {
                int temp = chkId[i];
                var article = db.ChapStories.Find(temp);
                db.ChapStories.Remove(article);
            }
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/ChapStories/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            ChapStory chapStory = db.ChapStories.Find(id);
            db.ChapStories.Remove(chapStory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
