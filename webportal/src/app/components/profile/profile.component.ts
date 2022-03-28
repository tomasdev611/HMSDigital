import {Component, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {concatMap, finalize} from 'rxjs/operators';
import {CustomValidators} from 'src/app/directives/custom-validators';
import {FileStorageService, ToastService, UserService} from 'src/app/services';
import {ImageCroppedEvent} from 'ngx-image-cropper';
import {base64StringToBlob} from 'blob-util';
import {NavbarSearchService} from 'src/app/services/navbar-search.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  userForm: FormGroup;
  passwordForm: FormGroup;

  attributeToVerify = '';
  verificationCode = '';
  imageChangedEvent: any = '';
  croppedImage: any = '';
  imageFormat = '';
  userImageUrl = '';
  user: any;

  formSubmit = false;
  verifyReqLoading = false;
  displayCodeConfirmation = false;
  resetLinkSubmit = false;
  passwordSubmit = false;
  imageSubmit = false;
  showCropper = false;
  imageLoading = false;

  @ViewChild('file') file: any;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastService: ToastService,
    private fileStorageService: FileStorageService,
    private navbarSearchService: NavbarSearchService
  ) {
    this.setUserForm();
    this.setPasswordForm();
  }

  setUserForm() {
    this.userForm = this.fb.group({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      email: new FormControl(null, Validators.required),
      phoneNumber: new FormControl(0),
      countryCode: new FormControl('+1'),
    });
  }

  setPasswordForm() {
    this.passwordForm = this.fb.group(
      {
        oldPassword: new FormControl(null, Validators.required),
        password: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/\d/, {hasNumber: true}),
            CustomValidators.patternValidator(/[A-Z]/, {hasUpperCase: true}),
            CustomValidators.patternValidator(/[a-z]/, {hasLowerCase: true}),
            CustomValidators.patternValidator(/[~!@#$%^&*()_[\]{};':"\\|,.<>\/?]/, {
              hasSpecialCharacters: true,
            }),
            Validators.minLength(8),
          ]),
        ],
        confirmPassword: [null, Validators.compose([Validators.required])],
      },
      {
        validators: CustomValidators.passwordMatchValidator,
      }
    );
  }

  ngOnInit(): void {
    const myInfo: any = localStorage.getItem('me');
    if (myInfo) {
      this.user = JSON.parse(myInfo);
      this.userForm.patchValue(this.user);
      this.getProfileImage();
    }
  }

  getProfileImage() {
    this.userService.getProfileUrl(this.user.id).subscribe((response: any) => {
      if (response?.downloadUrl) {
        this.userImageUrl = response.downloadUrl;
        this.navbarSearchService.userImageChanged(this.userImageUrl);
        this.imageLoading = true;
        if (this.userImageUrl !== this.user.profilePictureUrl) {
          this.updateUserCache(this.userImageUrl);
        }
      }
      this.removeFile();
    });
  }

  onSubmit(value) {
    this.userForm.controls.phoneNumber.setValue(parseInt(value.phoneNumber, 10) ?? 0);
    this.userForm.controls.countryCode.setValue(parseInt(value.countryCode, 10) ?? 0);
    this.formSubmit = true;
    this.userService
      .updateSelfUser(this.userForm.value)
      .pipe(
        finalize(() => {
          this.formSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.user = {
          ...this.user,
          ...response,
          ...{userRoles: this.user.userRoles},
        };
        localStorage.setItem('me', JSON.stringify(this.user));
        this.navbarSearchService.userInfoUpdated(this.user);
        this.toastService.showSuccess(`User updated successfully`);
      });
  }

  verify(attribute) {
    this.verifyReqLoading = true;
    this.attributeToVerify = attribute;
    const body = {
      verifyAttribute: this.attributeToVerify,
    };
    this.userService
      .sendVerificationCode(this.user.id, body)
      .pipe(
        finalize(() => {
          this.verifyReqLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.displayCodeConfirmation = true;
        this.toastService.showSuccess(
          `Verification Code sent to ${
            this.attributeToVerify === 'email_verify' ? 'Email' : 'Mobile'
          }`
        );
      });
  }

  onSubmitPassword(value: any) {
    this.passwordSubmit = true;
    const body = {
      oldPassword: value.oldPassword,
      newPassword: value.password,
    };
    this.userService
      .changeSelfPassword(body)
      .pipe(
        finalize(() => {
          this.passwordSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.passwordForm.reset();
        this.setPasswordForm();
        this.toastService.showSuccess(`Password changed successfully`);
      });
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
    const type = event?.target?.files[0]?.type?.split('/')[1];
    if (type && ['jpeg', 'png'].find(t => t === type)) {
      this.imageFormat = type;
      this.showCropper = true;
    } else {
      this.removeFile();
      this.toastService.showInfo(`Please select correct image format (ie. png, jpeg)`);
    }
  }
  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = event.base64;
  }
  imageLoaded() {
    // image loaded
  }
  cropperReady() {
    // cropper ready
  }
  loadImageFailed() {
    // show message
  }
  removeFile() {
    this.showCropper = false;
    this.croppedImage = '';
    if (this.file?.nativeElement) {
      this.file.nativeElement.value = '';
    }
  }
  upload() {
    const fileName = this.file?.nativeElement?.files[0]?.name;
    const fileType = this.file?.nativeElement?.files[0]?.type;
    const base64 = this.croppedImage.split(',')[1];
    const imageName = fileName;
    const imageBlob = base64StringToBlob(base64);
    const imageFile = new File([imageBlob], imageName, {type: fileType});
    const userImage = {
      name: fileName,
      description: '',
      contentType: fileType.substr(fileType.lastIndexOf('/') + 1),
      sizeInBytes: imageFile.size,
    };
    this.imageSubmit = true;
    this.userService
      .getUploadUrl(this.user.id, userImage)
      .pipe(
        concatMap((uploadResponse: any) => {
          return this.fileStorageService.storeFile(uploadResponse.uploadUrl, imageFile);
        }),
        concatMap((_: any) => {
          return this.userService.confirmImageUpload(this.user.id);
        })
      )
      .subscribe(
        (res: any) => {
          this.toastService.showSuccess('File Uploaded successfully');
          this.imageSubmit = false;
          this.getProfileImage();
        },
        error => {
          this.imageSubmit = false;
          this.removeFile();
          throw error;
        }
      );
  }
  deleteProfileImage() {
    this.imageSubmit = true;
    this.userService
      .deleteProfileImage(this.user.id)
      .pipe(
        finalize(() => {
          this.imageSubmit = false;
        })
      )
      .subscribe((response: any) => {
        this.userImageUrl = '';
        this.navbarSearchService.userImageChanged('');
        this.updateUserCache('');
        this.toastService.showSuccess(`Image deleted successfully`);
      });
  }
  updateUserCache(imageUrl) {
    this.user.profilePictureUrl = imageUrl;
    localStorage.setItem('me', JSON.stringify(this.user));
  }
  hideImgLoader() {
    this.imageLoading = false;
  }
}
