import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import type { AddUserShippingAddressRequest } from "../../../types/user/shipping-addresses/dtos/AddUserShippingAddressRequest";
import type { Province } from "../../../types/address/dtos/Province";
import type { Ward } from "../../../types/address/dtos/Ward";
import addressApi from "../../../api/addressApi";
import userShippingAddressApi from "../../../api/userShippingAddressApi";
import { parseApiError } from "../../../utils/error";

const schema = z.object({
  recipientName: z.string().min(1, "Vui lòng nhập tên người nhận"),
  recipientPhone: z.string().min(1, "Vui lòng nhập số điện thoại"),
  detailAddress: z.string().min(1, "Vui lòng nhập địa chỉ chi tiết"),
  provinceId: z.number().min(1, "Vui lòng chọn Tỉnh/Thành phố"),
  wardId: z.number().min(1, "Vui lòng chọn Xã/Phường"),
});

type FormData = z.infer<typeof schema>;

interface ShippingAddressFormProps {
  initialAddressId?: string | null;
  onSuccess: (action?: "create" | "update" | "delete") => void;
  onCancel: () => void;
}

export default function ShippingAddressForm({
  initialAddressId,
  onSuccess,
  onCancel,
}: ShippingAddressFormProps) {
  const [provinces, setProvinces] = useState<Province[]>([]);
  const [wards, setWards] = useState<Ward[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    setValue,
    watch,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const selectedProvinceId = watch("provinceId");

  // Fetch provinces on mount
  useEffect(() => {
    const fetchProvinces = async () => {
      try {
        const data = await addressApi.getProvinces();
        setProvinces(data);
      } catch (err) {
        console.error("Failed to fetch provinces", err);
      }
    };
    fetchProvinces();
  }, []);

  // Fetch address detail if update mode
  useEffect(() => {
    const fetchAddressDetail = async () => {
      // Wait for provinces to be loaded before setting form values
      if (!initialAddressId || provinces.length === 0) return;

      try {
        setLoading(true);
        const data =
          await userShippingAddressApi.getUserShippingAddressById(
            initialAddressId,
          );

        // Set targetWardId FIRST so the effect triggered by provinceId change sees it
        setTargetWardId(data.wardId);

        setValue("recipientName", data.recipientName);
        setValue("recipientPhone", data.recipientPhone);
        setValue("detailAddress", data.detailAddress);
        setValue("provinceId", data.provinceId);
      } catch (err) {
        setError(parseApiError(err, "Không thể tải thông tin địa chỉ").message);
      } finally {
        setLoading(false);
      }
    };

    fetchAddressDetail();
  }, [initialAddressId, setValue, provinces.length]);

  const [targetWardId, setTargetWardId] = useState<number | null>(null);

  // Fetch wards when province changes
  useEffect(() => {
    const fetchWards = async () => {
      if (!selectedProvinceId) {
        setWards([]);
        return;
      }

      // Reset ward when province changes
      // This might momentarily clear the ward during edit, but the next effect will restore it
      setValue("wardId", 0);

      try {
        const data = await addressApi.getWardsByProvinceId(selectedProvinceId);
        setWards(data);
      } catch (err) {
        console.error("Failed to fetch wards", err);
      }
    };

    fetchWards();
  }, [selectedProvinceId, setValue]);

  // Apply target ward when wards are loaded
  useEffect(() => {
    if (targetWardId && wards.length > 0) {
      const exists = wards.some((w) => w.id === targetWardId);
      if (exists) {
        setValue("wardId", targetWardId);
      }
      // Clear target after attempting to set
      setTargetWardId(null);
    }
  }, [wards, targetWardId, setValue]);

  const onSubmit = async (data: FormData) => {
    try {
      setLoading(true);
      setError(null);
      const requestData: AddUserShippingAddressRequest = {
        ...data,
      };

      if (initialAddressId) {
        await userShippingAddressApi.updateUserShippingAddress(
          initialAddressId,
          requestData,
        );
        onSuccess("update");
      } else {
        await userShippingAddressApi.addUserShippingAddress(requestData);
        onSuccess("create");
      }
    } catch (err) {
      setError(parseApiError(err, "Có lỗi xảy ra").message);
    } finally {
      setLoading(false);
    }
  };

  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

  const onDeleteClick = () => {
    setShowDeleteConfirm(true);
  };

  const onConfirmDelete = async () => {
    if (!initialAddressId) return;
    try {
      setLoading(true);
      await userShippingAddressApi.removeUserShippingAddress(initialAddressId);
      onSuccess("delete");
    } catch (err) {
      setError(parseApiError(err, "Không thể xóa địa chỉ").message);
      setShowDeleteConfirm(false); // Close confirm if failed to see error
    } finally {
      setLoading(false);
    }
  };

  const onCancelDelete = () => {
    setShowDeleteConfirm(false);
  };

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {error && (
          <div className="p-3 text-sm text-red-600 bg-red-50 dark:bg-red-900/10 rounded-lg">
            {error}
          </div>
        )}

        {/* Recipient Name */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Tên người nhận
          </label>
          <input
            {...register("recipientName")}
            type="text"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            placeholder="Nhập tên người nhận"
          />
          {errors.recipientName && (
            <p className="mt-1 text-xs text-red-500">
              {errors.recipientName.message}
            </p>
          )}
        </div>

        {/* Phone */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Số điện thoại
          </label>
          <input
            {...register("recipientPhone")}
            type="text"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            placeholder="Nhập số điện thoại"
          />
          {errors.recipientPhone && (
            <p className="mt-1 text-xs text-red-500">
              {errors.recipientPhone.message}
            </p>
          )}
        </div>

        <div className="grid grid-cols-2 gap-4">
          {/* Province */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Tỉnh/Thành phố
            </label>
            <select
              {...register("provinceId", { valueAsNumber: true })}
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            >
              <option value="0">Chọn Tỉnh/Thành</option>
              {provinces.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.name}
                </option>
              ))}
            </select>
            {errors.provinceId && (
              <p className="mt-1 text-xs text-red-500">
                {errors.provinceId.message}
              </p>
            )}
          </div>

          {/* Ward */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Xã/Phường
            </label>
            <select
              {...register("wardId", { valueAsNumber: true })}
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              disabled={!selectedProvinceId}
            >
              <option value="0">Chọn Xã/Phường</option>
              {wards.map((w) => (
                <option key={w.id} value={w.id}>
                  {w.name}
                </option>
              ))}
            </select>
            {errors.wardId && (
              <p className="mt-1 text-xs text-red-500">
                {errors.wardId.message}
              </p>
            )}
          </div>
        </div>

        {/* Detail Address */}
        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Địa chỉ chi tiết
          </label>
          <textarea
            {...register("detailAddress")}
            rows={3}
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-700 text-gray-900 dark:text-white resize-none"
            placeholder="Số nhà, tên đường..."
          />
          {errors.detailAddress && (
            <p className="mt-1 text-xs text-red-500">
              {errors.detailAddress.message}
            </p>
          )}
        </div>

        {/* Actions */}
        <div className="flex justify-end space-x-3 pt-2">
          {initialAddressId && (
            <button
              type="button"
              onClick={onDeleteClick}
              className="px-4 py-2 text-sm font-medium text-white bg-red-600 hover:bg-red-700 rounded-lg shadow-sm transition-colors mr-auto"
              disabled={loading}
            >
              Xóa
            </button>
          )}

          <button
            type="button"
            onClick={onCancel}
            className="px-4 py-2 text-sm font-medium text-gray-700 dark:text-gray-200 bg-white dark:bg-gray-800 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
            disabled={loading}
          >
            Hủy bỏ
          </button>
          <button
            type="button"
            onClick={handleSubmit(onSubmit)}
            className="px-4 py-2 text-sm font-medium text-white bg-brand-600 hover:bg-brand-700 rounded-lg shadow-sm transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={loading}
          >
            {loading
              ? "Đang xử lý..."
              : initialAddressId
                ? "Cập nhật"
                : "Thêm mới"}
          </button>
        </div>
      </form>

      {/* Confirmation Modal */}
      {showDeleteConfirm && (
        <div className="absolute inset-0 z-[110] flex items-center justify-center p-4 bg-black/50 backdrop-blur-sm rounded-xl animate-fade-in">
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-xl max-w-sm w-full animate-scale-up">
            <h3 className="text-lg font-bold text-gray-900 dark:text-white mb-2">
              Xác nhận xóa
            </h3>
            <p className="text-gray-600 dark:text-gray-300 mb-6">
              Bạn có chắc chắn muốn xóa địa chỉ này không? Hành động này không
              thể hoàn tác.
            </p>
            <div className="flex justify-end space-x-3">
              <button
                onClick={onCancelDelete}
                className="px-4 py-2 text-sm font-medium text-gray-700 dark:text-gray-200 bg-gray-100 dark:bg-gray-700 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-600 transition-colors"
              >
                Hủy
              </button>
              <button
                onClick={onConfirmDelete}
                className="px-4 py-2 text-sm font-medium text-white bg-red-600 hover:bg-red-700 rounded-lg shadow-sm transition-colors"
                disabled={loading}
              >
                {loading ? "Đang xóa..." : "Xóa"}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
