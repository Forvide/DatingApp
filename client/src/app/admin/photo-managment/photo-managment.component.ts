import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-managment',
  templateUrl: './photo-managment.component.html',
  styleUrls: ['./photo-managment.component.css']
})
export class PhotoManagmentComponent implements OnInit {

  photosToApprove: Partial<Photo[]> = [];

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotoForApproval().subscribe(result => {
      this.photosToApprove = result.result;
    })
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe(() => {
      this.photosToApprove = this.photosToApprove.filter(p => p.id !== photoId);
    });
  }

  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe(() => {
      this.photosToApprove.filter(p => p.id !== photoId);
    });
  }
}
