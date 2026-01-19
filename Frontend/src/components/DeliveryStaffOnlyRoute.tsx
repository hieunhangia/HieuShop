import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { PAGES } from '../config/page';
import { USER_ROLES } from '../config/userRoles';
import { Loader } from 'lucide-react';

export default function DeliveryStaffOnlyRoute() {
    const { user, isLoading } = useAuth();

    if (isLoading) {
        return (
            <div className="flex justify-center items-center min-h-screen">
                <Loader className="w-8 h-8 text-brand-600 animate-spin" />
            </div>
        );
    }

    if (user && user.roles && user.roles.includes(USER_ROLES.DELIVERY_STAFF)) {
        return <Outlet />;
    }

    if (!user) {
        return <Navigate to={PAGES.IDENTITY.LOGIN.PATH} replace />;
    }

    return <Navigate to={PAGES.HOME.PATH} replace />;
}
